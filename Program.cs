using System;
using System.Linq;
using System.Threading;
using static System.Console;
Clear();
SetWindowSize(80, 25);
SetBufferSize(80, 25);

// Отрисовка рамочки
HorizontalLine upLine = new HorizontalLine(0, 78, 0, '+');
HorizontalLine downLine = new HorizontalLine(0, 78, 24, '+');
VerticalLine leftLine = new VerticalLine(0, 24, 0, '+');
VerticalLine rightLine = new VerticalLine(0, 24, 78, '+');
upLine.Draw();
downLine.Draw();
leftLine.Draw();
rightLine.Draw();

Point p = new Point(4, 5, '*');
Snake snake = new Snake(p, 4, Direction.RIGHT);
snake.Draw();

FoodCreator foodCreator = new FoodCreator(80, 25, '$');
Point food = foodCreator.CreateFood();
food.Draw();


while(true) {
	if (snake.Eat(food)) {
		food = foodCreator.CreateFood();
		food.Draw();
	} else {
		snake.Move();
	}
	
	Thread.Sleep(100);

	if (KeyAvailable) {
		ConsoleKeyInfo key = ReadKey();
		snake.HadleKey(key.Key);
	}
}


class FoodCreator {
	int mapWidth;
	int mapHeight;
	char sym;

	Random random = new Random();

	public FoodCreator (int mapWidth, int mapHeight, char sym) {
		this.mapWidth = mapWidth;
		this.mapHeight = mapHeight;
		this.sym = sym;
	}
	public Point CreateFood () {
		int x = random.Next(2, mapWidth - 2);
		int y = random.Next(2, mapHeight - 2);
		return new Point(x, y, sym);
	}
}

class Point {
	public int x;
	public int y;
	public char sym;

	public Point () {
	}
	public Point (int x, int y, char sym) {
		this.x = x;
		this.y = y;
		this.sym = sym;
	}
	public Point (Point p) {
		x = p.x;
		y = p.y;
		sym = p.sym;
	}
	public void Move(int offset, Direction direction) {
		if (direction == Direction.RIGHT)		x += offset;
		else if (direction == Direction.LEFT)	x -= offset;
		else if (direction == Direction.UP)		y -= offset;
		else									y += offset;
	}
	public bool IsHit (Point p) {
		return p.x == this.x && p.y == this.y;
	}
	public void Draw() {
		SetCursorPosition(x, y);
		Write(sym);
	}
	public void Clear() {
		sym = ' ';
		Draw();
	}
	public override string ToString()
	{
		return x + ", " + y + ", " + sym;
	}
}

class HorizontalLine : Figure {
	public HorizontalLine(int xLeft, int xRight, int y, char sym) {
		pList = new List<Point>();
		for (int x = xLeft; x <= xRight; x ++) {
			Point p = new Point(x, y, sym);
			pList.Add(p);
		}
	}
}

class VerticalLine : Figure {
	public VerticalLine(int yUp, int yDown, int x, char sym) {
		pList = new List<Point>();
		for (int y = yUp; y <= yDown; y ++) {
			Point p = new Point(x, y, sym);
			pList.Add(p);
		}
	}
}

class Snake : Figure {
	Direction direction;
	public Snake(Point tail, int length, Direction _direction) {
		direction = _direction;
		pList = new List<Point>();
		for (int i = 0; i < length; i ++) {
			Point p = new Point(tail);
			p.Move(i, direction);
			pList.Add(p);
		}
	}
	internal void Move() {
		Point tail = pList.First();
		pList.Remove(tail);
		Point head = GetNextPoint();
		pList.Add(head);

		tail.Clear();
		head.Draw();
	}
	public Point GetNextPoint() {
		Point head = pList.Last();
		Point nextPoint = new Point (head);
		nextPoint.Move(1, direction);
		return nextPoint;
	}
	public void HadleKey (ConsoleKey key) {
		if (key == ConsoleKey.LeftArrow)		direction = Direction.LEFT;
		else if (key == ConsoleKey.RightArrow)	direction = Direction.RIGHT;
		else if (key == ConsoleKey.DownArrow)	direction = Direction.DOWN;
		else if (key == ConsoleKey.UpArrow)		direction = Direction.UP;
	}
	internal bool Eat (Point food) {
		Point head = GetNextPoint();
		if (head.IsHit(food)) {
			food.sym = head.sym;
			pList.Add(food);
			return true;
		} else		return false;
	}
}

class Figure {
	protected List<Point> pList;
	public void Draw () {
		foreach(Point p in pList) {
			p.Draw();
		}
	}
}

enum Direction {
	LEFT,
	RIGHT,
	UP,
	DOWN
}

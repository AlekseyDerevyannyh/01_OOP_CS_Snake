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
upLine.Drow();
downLine.Drow();
leftLine.Drow();
rightLine.Drow();

Point p = new Point(4, 5, '*');
Snake snake = new Snake(p, 4, Direction.RIGHT);
snake.Drow();

while(true) {
	if (KeyAvailable) {
		ConsoleKeyInfo key = ReadKey();
		snake.HadleKey(key.Key);
	}
	Thread.Sleep(100);
	snake.Move();
}

class Point {
	public int x;
	public int y;
	public char sym;

	public Point () {
	}
	public Point (int _x, int _y, char _sym) {
		x = _x;
		y = _y;
		sym = _sym;
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
	public void Draw() {
		SetCursorPosition(x, y);
		Write(sym);
	}
	public void Clear() {
		sym = ' ';
		Draw();
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
}

class Figure {
	protected List<Point> pList;
	public void Drow () {
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

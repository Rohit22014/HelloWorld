using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace MyCommand
{
    /*
     This Program uses a Command Design Pattern as shown in the example
     the undo stack is where the history of all executed commands is stored.
     This allows us to use redo and undo commands in the program.
     The Program allows a user to create each shape and be able to store it in a SVG file.
     */

    class Program
    {
        // The Canvas (Receiver) class - holds a list of shapes (model)
        // Note - creating this class to hid how it is implemented and provide
        //        add and remove methods (which are just push and pop operations)
        public class Canvas
        {
            // Use a stack here only because we are orking with Stack<T> classes
            // I tend to prefer List<T> classes and I have control over manipulating
            // the data structure - however, the stack data structure works fine here
            private Stack<Shape> canvas = new Stack<Shape>();

            public void Add(Shape s)
            {
                canvas.Push(s);
                Console.WriteLine("Added Shape to canvas: {0}" + Environment.NewLine, s);
            }
            public Shape Remove()
            {
                Shape s = canvas.Pop();
                Console.WriteLine("Removed Shape from canvas: {0}" + Environment.NewLine, s);
                return s;
            }

            public Canvas()
            {
                Console.WriteLine("\nCreated a new Canvas!"); Console.WriteLine();
            }

            public void Createfile()
            {
                string path = "../sample.svg";
                if (!File.Exists(path))
                {
                    File.CreateText(path);
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(String.Format(@"<svg height=""400"" width=""400"" xmlns=""http://www.w3.org/2000/svg"">"));
                    foreach (Shape s in canvas)
                    {
                        sw.WriteLine("".PadLeft(3, ' ') + s);
                    }
                    sw.WriteLine("</svg>");
                    sw.Close();
                }
            }

            public override string ToString()
            {
                String str = "Canvas (" + canvas.Count + " elements): " + Environment.NewLine + Environment.NewLine;
                foreach (Shape s in canvas)
                {
                    str += "   > " + s + Environment.NewLine;
                }
                return str;
            }

        }

        // Abstract Shape class
        public abstract class Shape
        {
            public override string ToString()
            {
                return "Shape!";
            }
        }

        // Circle Shape class
        public class Circle : Shape
        {

            public int X { get; private set; }
            public int Y { get; private set; }
            public int R { get; private set; }

            public Circle(int x, int y, int r)
            {
                X = x; Y = y; R = r;
            }

            public override string ToString()
            {
                /*
                return "Circle [x: " + X + ", y: " + Y + ", r: " + R + "]";
            */
                string dispSVG = String.Format(@"<circle cx=""{0}"" cy=""{1}"" r=""{2}""/>", X, Y, R);
                return dispSVG;            
            }
        }
        // Rectangle Shape class
        public class Rectangle : Shape 
        {
            public int Height { get; set; } // circle centre x-coordinate
            public int Width { get; set; } // circle centre y-coordinate
            public int RX { get; set; } // circle radius
            public int RY { get; set; }
            
            public Rectangle(int height, int width, int rx, int ry)
            {
                Height = height;
                Width = width;
                RX = rx;
                RY = ry;
            }

            public override string ToString()
            {
                /*return "Rectangle [Height: " + Height + ", Width: " + Width + ", rx: " + RX + ", ry : " + RY + "]";*/
                string dispSVG =
                    String.Format(
                        @"<rect width=""{0}"" height=""{1}"" rx=""{2}"" ry=""{3}""/>", Height, Width,
                        RX, RY);
                return dispSVG; 
            }
        }
        public class Ellipse : Shape 
        {
            public int CX { get; set; } // circle radius
            public int CY { get; set; }
            public int RX { get; set; } // circle radius
            public int RY { get; set; }
            
            public Ellipse(int cx, int cy, int rx, int ry)
            {
                CX = cx;
                CY = cy;
                RX = rx;
                RY = ry;
            }
            public override string ToString()
            {
                /*
                return "Ellipse [cx: " + CX + ", cy: " + CY + ", rx: " + RX + ", ry: " + RY + "]";
            */
                string dispSVG =
                    String.Format(
                        @"<ellipse cx=""{0}"" cy=""{1}"" rx=""{2}"" ry=""{3}"" />", CX, CY,
                        RX, RY);
                return dispSVG; 
            }
        }
        public class Line : Shape 
        {
            public int X1 { get; set; } // circle centre x-coordinate
            public int Y1 { get; set; } // circle centre y-coordinate
            public int X2 { get; set; } // circle radius
            public int Y2 { get; set; }
            
            public Line(int x1, int y1, int x2, int y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }
            public override string ToString()
            {
                /*
                return "Line [x1: " + X1 + " x2: " + X2 + " y1: " + Y1 + " y2: " + Y2 + "]";
            */
                string dispSVG =
                    String.Format(
                        @"<line x1=""{0}"" y1=""{1}"" x2=""{2}"" y2=""{3}"" />", X1, Y1, X2, Y2);
                return dispSVG; 
            }
        }
        public class Path : Shape
        {
            public int M1 { get; set; } // circle centre x-coordinate
            public int L1 { get; set; } // circle centre y-coordinate
            public int V1 { get; set; } // circle radius
            public int H1{ get; set; }
            public int M2 { get; set; } // circle centre x-coordinate
            public int L2 { get; set; } // circle centre y-coordinate
            public int V2 { get; set; } // circle radius
            public int H2 { get; set; }

            public Path(int m1, int m2, int l1, int l2, int v1, int v2, int h1, int h2)
            {
                M1 = m1;
                L1 = l1;
                V1 = v1;
                H1 = h1;
                M2 = m2;
                L2 = l2;
                V2 = v2;
                H2 = h2;
            }
            public override string ToString()
            {
                /*
                return "Path [M: " + M1 + ","+ M2 + ", L: " + L1 +","+ L2 + ", V: " + V1 + "," + V2 + ", H: " + H1 + "," + H2 + "]";
            */
                string dispSVG =
                    /*String.Format(
                        @"<path d=M""{0}"",""{1}"" L""{2}"",""{3}"" V""{4}"",""{5}"" H""{6}"",""{7}"" />", M1, M2, L1, L2, V1, V2, H1, H2);*/
                    "<path d=" + "\"" + "M" + M1 + "," + M2 + " L" + L1 + "," + L2 + " V" + V1 + "," + V2 + " H" + H1 + "," + H2 + "\"" + " />";
                return  dispSVG; 
            }
        }
        public class Polygon : Shape
        {
            public int P1 { get; set; } // circle centre y-coordinate
            public int P2 { get; set; } // circle radius
            public int P3{ get; set; }
            public int P4 { get; set; } // circle centre x-coordinate
            public int P5 { get; set; } // circle centre y-coordinate 
            public int P6 { get; set; }

            public Polygon(int p1, int p2, int p3, int p4, int p5, int p6)
            {
                P1 = p1;
                P2 = p2;
                P3 = p3;
                P4 = p4;
                P5 = p5;
                P6 = p6;
            }
            public override string ToString()
            {
                /*return "Polygon [Points: "+ P1+" " +P2+ " " + P3+" " + P4 + " " + P5 + "]";*/
                string dspSVG = "<polygon points=" + "\"" + P1 + ", " + P2 + " " + P3 + ", " + P4 + " " + P5 + ", " + P6 +"\"" + " />";
                return dspSVG;
            }
        }
        public class Polyline : Shape 
        {
            public int P1 { get; set; } // circle centre y-coordinate
            public int P2 { get; set; } // circle radius
            public int P3{ get; set; }
            public int P4 { get; set; } // circle centre x-coordinate
            public int P5 { get; set; } // circle centre y-coordinate
            public int P6 { get; set; }


            public Polyline(int p1, int p2, int p3, int p4, int p5, int p6)
            {
                P1 = p1;
                P2 = p2;
                P3 = p3;
                P4 = p4;
                P5 = p5;
                P6 = p6;
            }
            public override string ToString()
            {
                /*return "Polyline [Points: "+ P1+" " +P2+ " " + P3+" " + P4 + " " + P5 + "]";*/
                string dspSVG = "<polyline points=" + "\"" + P1 + ", " + P2 + " " + P3 + ", " + P4 + " " + P5 + ", " + P6 + "\"" + " />";
                return dspSVG;
            }
        }

        // The User (Invoker) Class
        public class User
        {
            private Stack<Command> undo;
            private Stack<Command> redo;

            public int UndoCount { get => undo.Count; }
            public int RedoCount { get => undo.Count; }
            public User()
            {
                Reset();
                Console.WriteLine("Created a new User!"); Console.WriteLine();
            }
            public void Reset()
            {
                undo = new Stack<Command>();
                redo = new Stack<Command>();
            }

            public void Action(Command command)
            {
                // first update the undo - redo stacks
                undo.Push(command);  // save the command to the undo command
                redo.Clear();        // once a new command is issued, the redo stack clears

                // next determine  action from the Command object type
                // this is going to be AddShapeCommand or DeleteShapeCommand
                Type t = command.GetType();
                if (t.Equals(typeof(AddShapeCommand)))
                {
                    Console.WriteLine("Command Received: Add new Shape!" + Environment.NewLine);
                    command.Do();
                }
                if (t.Equals(typeof(DeleteShapeCommand)))
                {
                    Console.WriteLine("Command Received: Delete last Shape!" + Environment.NewLine);
                    command.Do();
                }
            }

            // Undo
            public void Undo()
            {
                Console.WriteLine("Undoing operation!"); Console.WriteLine();
                if (undo.Count > 0)
                {
                    Command c = undo.Pop(); c.Undo(); redo.Push(c);
                }
            }

            // Redo
            public void Redo()
            {
                Console.WriteLine("Redoing operation!"); Console.WriteLine();
                if (redo.Count > 0)
                {
                    Command c = redo.Pop(); c.Do(); undo.Push(c);
                }
            }

        }


        // Abstract Command (Command) class - commands can do something and also undo
        public abstract class Command
        {
            public abstract void Do();     // what happens when we execute (do)
            public abstract void Undo();   // what happens when we unexecute (undo)
        }


        // Add Shape Command - it is a ConcreteCommand Class (extends Command)
        // This adds a Shape (Circle) to the Canvas as the "Do" action
        public class AddShapeCommand : Command
        {
            Shape shape;
            Canvas canvas;

            public AddShapeCommand(Shape s, Canvas c)
            {
                shape = s;
                canvas = c;
            }

            // Adds a shape to the canvas as "Do" action
            public override void Do()
            {
                canvas.Add(shape);
            }
            // Removes a shape from the canvas as "Undo" action
            public override void Undo()
            {
                shape = canvas.Remove();
            }

        }

        // Delete Shape Command - it is a ConcreteCommand Class (extends Command)
        // This deletes a Shape (Circle) from the Canvas as the "Do" action
        public class DeleteShapeCommand : Command
        {

            Shape shape;
            Canvas canvas;

            public DeleteShapeCommand(Canvas c)
            {
                canvas = c;
            }

            // Removes a shape from the canvas as "Do" action
            public override void Do()
            {
                shape = canvas.Remove();
            }

            // Restores a shape to the canvas a an "Undo" action
            public override void Undo()
            {
                canvas.Add(shape);
            }
        }

        //
        // Entry point into application
        //
        static void Main()
        {
            bool stop = false;
            User user = new User();
            Canvas canvas = new Canvas();
            Random rnd = new Random();
            while (!stop)
            {
                string a = Console.ReadLine();
                if (a == "H")
                {
                    Console.WriteLine("Commands: ");
                    Console.WriteLine("H            Help - displays this message");
                    Console.WriteLine("A            Add shape to canvas");
                    Console.WriteLine("U            Undo last operation");
                    Console.WriteLine("R            Redo last operations");
                    Console.WriteLine("C            Clear canvas");
                    Console.WriteLine("S            Save Canvas in a SVG file");
                    Console.WriteLine("Q            Quit application");
                }
                else if (a == "A")
                {
                    Console.WriteLine("What would you like to add?");
                    Console.WriteLine("options:");
                    Console.WriteLine("C        Circle");
                    Console.WriteLine("E        Ellipse");
                    Console.WriteLine("L        Line");
                    Console.WriteLine("P        Path");
                    Console.WriteLine("O        Polygon");
                    Console.WriteLine("Y        Polyline");
                    Console.WriteLine("R        Rectangle");
                    string b = Console.ReadLine();
                    if (b == "Circle")
                    {
                        user.Action(new AddShapeCommand(new Circle(rnd.Next(1, 500), rnd.Next(1, 500), rnd.Next(1, 500)), canvas));
                    }
                    else if (b == "Rectangle")
                    {
                        user.Action(new AddShapeCommand(new Rectangle(rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500)),canvas));
                    }
                    else if (b == "Ellipse")
                    {
                       user.Action(new AddShapeCommand(new Ellipse(rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500)),canvas)); 
                    }
                    else if (b == "Line")
                    {
                      user.Action(new AddShapeCommand(new Line(rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500)),canvas)); 
                    }
                    else if (b == "Path")
                    {
                        user.Action(new AddShapeCommand(new Path(rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500),rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500)), canvas));
                    }
                    else if (b == "Polygon")
                    {
                        user.Action(new AddShapeCommand(new Polygon(rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500)), canvas));
                    }
                    else if (b == "Polyline")
                    {
                        user.Action(new AddShapeCommand(new Polyline(rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500), rnd.Next(1,500)), canvas));
                    }
                    else
                    {
                        Console.WriteLine("Not a shape!");
                    }
                }
                else if (a == "U")
                {
                    user.Undo();
                }
                else if (a == "R")
                {
                    user.Redo();
                }
                else if (a == "D")
                {
                    Console.WriteLine(canvas);
                }
                else if (a == "S")
                {
                    canvas.Createfile();
                    Console.WriteLine("File created");
                }
                else if (a == "Q")
                {
                    Console.WriteLine("Program ended");
                    stop = true;
                    break;
                }
                else
                {
                    Console.WriteLine("Not a input. Try again!");
                }
            }
        }
    }
    
}
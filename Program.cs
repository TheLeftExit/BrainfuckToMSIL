using System.Reflection.Emit;

namespace TheLeftExit.Brainfuck {
    static class Program {
        static void Main(string[] args) {
            HelloWorldTest();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        static void HelloWorldTest() {
            DynamicMethod myMethod = new DynamicMethod("MyMethod", null, Type.EmptyTypes);
            BrainfuckEmit emit = new BrainfuckEmit(myMethod.GetILGenerator());

            emit.Init();
            foreach(char c in "++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.") {
                switch (c) {
                    case '>': emit.IncPtr(); break;
                    case '<': emit.DecPtr(); break;
                    case '+': emit.Inc(); break;
                    case '-': emit.Dec(); break;
                    case '.': emit.Write(); break;
                    case ',': emit.Read(); break;
                    case '[': emit.BeginWhile(); break;
                    case ']': emit.EndWhile(); break;
                }
            }
            emit.End();

            Action myAction = myMethod.CreateDelegate<Action>();
            myAction();
        }
    }
}
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static System.Reflection.Emit.OpCodes;

#nullable disable

namespace TheLeftExit.Brainfuck {
    public record struct WhileLoop(Label Start, Label End);

    public class BrainfuckEmit {
        private ILGenerator _generator;
        private Stack<WhileLoop> _loops;

        private static MethodInfo __System_Console_get_In;
        private static MethodInfo __System_Console_get_Out;
        private static MethodInfo __System_IO_TextReader_Read;
        private static MethodInfo __System_IO_TextWriter_Write;

        static BrainfuckEmit() {
            __System_Console_get_In = typeof(Console).GetProperty("In").GetMethod;
            __System_Console_get_Out = typeof(Console).GetProperty("Out").GetMethod;
            __System_IO_TextReader_Read = typeof(TextReader).GetMethod("Read", Type.EmptyTypes);
            __System_IO_TextWriter_Write = typeof(TextWriter).GetMethod("Write", new Type[] { typeof(char) });
        }

        public BrainfuckEmit(ILGenerator generator) {
            _generator = generator;
            _loops = new();
        }

        public void Init() {
            _generator.DeclareLocal(typeof(TextReader));
            _generator.DeclareLocal(typeof(TextWriter));
            _generator.DeclareLocal(typeof(byte*));

            _generator.Emit(Call, __System_Console_get_In);
            _generator.Emit(Stloc_0);

            _generator.Emit(Call, __System_Console_get_Out);
            _generator.Emit(Stloc_1);

            _generator.Emit(Ldc_I4, 30000);
            _generator.Emit(Conv_U);
            _generator.Emit(Localloc);
            _generator.Emit(Stloc_2);
        }

        public void IncPtr() {
            _generator.Emit(Ldloc_2);

            _generator.Emit(Ldc_I4_1);
            _generator.Emit(Add);

            _generator.Emit(Stloc_2);
        }

        public void DecPtr() {
            _generator.Emit(Ldloc_2);

            _generator.Emit(Ldc_I4_1);
            _generator.Emit(Sub);

            _generator.Emit(Stloc_2);
        }

        public void Inc() {
            _generator.Emit(Ldloc_2);

            _generator.Emit(Dup);
            _generator.Emit(Ldind_U1);
            _generator.Emit(Ldc_I4_1);
            _generator.Emit(Add);
            _generator.Emit(Conv_U1); 
            _generator.Emit(Stind_I1);
        }

        public void Dec() {
            _generator.Emit(Ldloc_2);

            _generator.Emit(Dup);
            _generator.Emit(Ldind_U1);
            _generator.Emit(Ldc_I4_1);
            _generator.Emit(Sub);
            _generator.Emit(Conv_U1);
            _generator.Emit(Stind_I1);
        }

        public void Write() {
            _generator.Emit(Ldloc_1);
            _generator.Emit(Ldloc_2);
            _generator.Emit(Ldind_U1);
            _generator.Emit(Callvirt, __System_IO_TextWriter_Write);
        }

        public void Read() {
            _generator.Emit(Ldloc_2);
            _generator.Emit(Ldloc_0);
            _generator.Emit(Callvirt, __System_IO_TextReader_Read);
            _generator.Emit(Conv_U1);
            _generator.Emit(Stind_I1);
        }

        public void BeginWhile() {
            WhileLoop loop = new WhileLoop();
            loop.End = _generator.DefineLabel();
            _generator.Emit(Br, loop.End);
            loop.Start = _generator.DefineLabel();
            _generator.MarkLabel(loop.Start);
            _loops.Push(loop);
        }

        public void EndWhile() {
            WhileLoop loop = _loops.Pop();
            _generator.MarkLabel(loop.End);
            _generator.Emit(Ldloc_2);
            _generator.Emit(Ldind_U1);
            
            
            _generator.Emit(Brtrue, loop.Start);
        }

        public void End() {
            _generator.Emit(Ret);
        }
    }
}
using System.CodeDom;
using System.Reflection;

namespace SafeMapper.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection.Emit;

    using SafeMapper.Configuration;

    public class ConverterFactory : IConverterFactory
    {
        private readonly IMapConfiguration mapCfg;

        public ConverterFactory() : this(new MapConfiguration())
        {
        }

        public ConverterFactory(IMapConfiguration configuration)
        {
            this.mapCfg = configuration;
        }

        public IMapConfiguration Configuration
        {
            get
            {
                return this.mapCfg;
            }
        }

        public Func<object, object> CreateDelegate(Type fromType, Type toType)
        {
            return this.CreateDelegate(fromType, toType, CultureInfo.CurrentCulture);
        }

        public Func<object, object> CreateDelegate(Type fromType, Type toType, IFormatProvider provider)
        {
            var convertDynamicMethod = new DynamicMethod(
                "ConvertFrom" + fromType.Name + "To" + toType.Name + "NonGeneric",
                typeof(object),
                new[] { typeof(IFormatProvider), typeof(object) },
                typeof(ConverterFactory).Module,
                true);

            var il = new ILGeneratorAdapter(this.mapCfg);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(fromType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, fromType); // cast input to correct type
            il.EmitConvertValue(fromType, toType, new HashSet<Type>());
            il.Emit(OpCodes.Box, toType);
            il.Emit(OpCodes.Ret);

            return (Func<object, object>)CompileDynamicMethod<Func<object, object>>(convertDynamicMethod, il.Instructions, provider);
            return (Func<object, object>)convertDynamicMethod.CreateDelegate(typeof(Func<object, object>), provider);
        }

        public Converter<TFrom, TTo> CreateDelegate<TFrom, TTo>()
        {
            return this.CreateDelegate<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public Converter<TFrom, TTo> CreateDelegate<TFrom, TTo>(IFormatProvider provider)
        {
            var toType = typeof(TTo);
            var fromType = typeof(TFrom);

            var convertDynamicMethod = new DynamicMethod(
                "ConvertFrom" + fromType.Name + "To" + toType.Name,
                toType,
                new[] { typeof(IFormatProvider), fromType },
                typeof(ConverterFactory).Module, 
                true);

            var il = new ILGeneratorAdapter(this.mapCfg);

            il.Emit(OpCodes.Ldarg_1);
            il.EmitConvertValue(fromType, toType, new HashSet<Type>());
            il.Emit(OpCodes.Ret);

            return (Converter<TFrom, TTo>)CompileDynamicMethod<Converter<TFrom, TTo>>(convertDynamicMethod, il.Instructions, provider);

            //return (Converter<TFrom, TTo>)convertDynamicMethod.CreateDelegate(typeof(Converter<TFrom, TTo>), provider);
        }


        private Delegate CompileDynamicMethod<TDelegate>(DynamicMethod dynamicMethod, ILInstruction[] instructions, IFormatProvider provider)
        {
            var ilGenerator = dynamicMethod.GetILGenerator();
            foreach (var instruction in instructions)
            {
                this.EmitInstruction(ilGenerator, instruction);
            }

            return dynamicMethod.CreateDelegate(typeof(TDelegate), provider);
        } 

        private void EmitInstruction(ILGenerator ilGenerator, ILInstruction instruction)
        {
            switch (instruction.InstructionType)
            {
                case ILInstructionType.DeclareLocal:
                    var localWrapper = ((LocalBuilderWrapper)instruction.Argument);
                    var local = ilGenerator.DeclareLocal(localWrapper.LocalType);
                    localWrapper.LocalBuilder = local;
                    break;
                case ILInstructionType.DefineLabel:
                    var labelWrapper = ((LabelWrapper)instruction.Argument);
                    var label = ilGenerator.DefineLabel();
                    labelWrapper.Label = label;
                    break;
                case ILInstructionType.MarkLabel:
                    ilGenerator.MarkLabel(((LabelWrapper)instruction.Argument).Label);
                    break;
                case ILInstructionType.OpCode:
                    if (instruction.Argument == null)
                    {
                        ilGenerator.Emit(instruction.OpCode);
                    }
                    else if (instruction.ArgumentType == typeof(LocalBuilderWrapper))
                    {
                        ilGenerator.Emit(instruction.OpCode, ((LocalBuilderWrapper)instruction.Argument).LocalBuilder);
                    }
                    else if (instruction.ArgumentType == typeof(LabelWrapper))
                    {
                        ilGenerator.Emit(instruction.OpCode, ((LabelWrapper)instruction.Argument).Label);
                    }
                    else if (instruction.ArgumentType == typeof(MethodInfo))
                    {
                        ilGenerator.EmitCall(instruction.OpCode, (MethodInfo)instruction.Argument, null);
                    }
                    else if (instruction.ArgumentType == typeof(int))
                    {
                        ilGenerator.Emit(instruction.OpCode, (int)instruction.Argument);
                    }
                    else if (instruction.ArgumentType == typeof(long))
                    {
                        ilGenerator.Emit(instruction.OpCode, (long)instruction.Argument);
                    }
                    else if (instruction.ArgumentType == typeof(double))
                    {
                        ilGenerator.Emit(instruction.OpCode, (double)instruction.Argument);
                    }
                    else if (instruction.ArgumentType == typeof(float))
                    {
                        ilGenerator.Emit(instruction.OpCode, (float)instruction.Argument);
                    }
                    else if (instruction.ArgumentType == typeof(string))
                    {
                        ilGenerator.Emit(instruction.OpCode, (string)instruction.Argument);
                    }
                    else if (instruction.ArgumentType == typeof(Type))
                    {
                        ilGenerator.Emit(instruction.OpCode, (Type)instruction.Argument);
                    }
                    else if (instruction.ArgumentType == typeof(FieldInfo))
                    {
                        ilGenerator.Emit(instruction.OpCode, (FieldInfo)instruction.Argument);
                    }
                    else if (instruction.ArgumentType == typeof(ConstructorInfo))
                    {
                        ilGenerator.Emit(instruction.OpCode, (ConstructorInfo)instruction.Argument);
                    }

                    break;
            }
        }
    }
}

.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
{
    .method public static valuetype Hw2.CalculatorOperation 
    ParseOperation(
        string operation
    )
    {
        .maxstack 2
        
        // plus operand check
        ldarg operation
        ldstr "+"
        call bool string::op_Equality(string, string)
        brtrue.s PLUS
        
        // minus operand check
        ldarg operation
        ldstr "-"
        call bool string::op_Equality(string, string)
        brtrue.s MINUS
        
        // multiply operand check
        ldarg operation
        ldstr "*"
        call bool string::op_Equality(string, string)
        brtrue.s MULTIPLY
        
        // divide operand check
        ldarg operation
        ldstr "/"
        call bool string::op_Equality(string, string)
        brtrue.s DIVIDE
        
        // default
        br.s UNDEFINED   
        
        PLUS:
            ldc.i4.0
            br.s RESULT
        MINUS:
            ldc.i4.1
            br.s RESULT
        MULTIPLY:
            ldc.i4.2
            br.s RESULT
        DIVIDE:
            ldc.i4.3
            br.s RESULT
        UNDEFINED: 
            ldc.i4.4
            br.s RESULT
        
        RESULT: ret
    }
    
    .method private static bool
    IsArgLengthSupported(
        string[] args
    ) cil managed
    {
        .maxstack 2
        
        ldarg args
        ldlen
        conv.i4
        ldc.i4.3
        ceq
        ret
    }
  
    .method public static void
    ParseCalcArguments(
        string[] args,
        [out] float64& val1,
        [out] valuetype Hw2.CalculatorOperation& operation,
        [out] float64& val2
    ) cil managed
    {
        .maxstack 3
        
        .locals init (
            float64 tempVal,
            valuetype Hw2.CalculatorOperation tempOp
        )
    
        ldarg args
        call bool Hw2.Parser::IsArgLengthSupported(string[])
        brfalse.s THROW_ARGUMENT_EXCEPTION

        ldarg args
        ldc.i4.0 // store an index
        ldelem.ref // address by stored index
        call float64 Hw2.Parser::ParseOperand(string)
        
        stloc tempVal 
        ldarg val1 // loading address of an out argument
        ldloc tempVal
        stind.r8
        
        ldarg args
        ldc.i4.1
        ldelem.ref
        call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
        dup
        ldc.i4.4 // CalculatorOperation.Undefined
        ceq
        brtrue.s THROW_INVALID_OPERATION_EXCEPTION
         
        stloc tempOp
        ldarg operation
        ldloc tempOp
        stind.i1
        
        ldarg args
        ldc.i4.2
        ldelem.ref
        call float64 Hw2.Parser::ParseOperand(string)
        
        stloc tempVal
        ldarg val2
        ldloc tempVal
        stind.r8
        
        ret
        
        THROW_ARGUMENT_EXCEPTION:
            ldstr "Недостаточно аргументов, их должно быть не менее трех."
            ldstr "args"
            newobj instance void [System.Runtime]System.ArgumentException::.ctor(string, string)
            throw
        
        THROW_INVALID_OPERATION_EXCEPTION:
            ldstr "operation"
            newobj instance void [System.Runtime]System.InvalidOperationException::.ctor(string)
            throw
    }
    
    .method public static float64 
    ParseOperand(
        string val
    )
    {
        .maxstack 2
        
        .locals init (
            float64 result
        )
        
        ldarg val
        ldloca.s result // load address of result
        call bool float64::TryParse(string, float64&)
        brfalse.s THROW_ARGUMENT_EXCEPTION
        
        ldloc result
        ret
        
        THROW_ARGUMENT_EXCEPTION:
            ldstr "операнд не является числом."
            newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
            throw
    }
}

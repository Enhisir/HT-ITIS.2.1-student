.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator
{
    .method public static float64
    Calculate(
        float64 value1, 
        valuetype Hw2.CalculatorOperation operation,
        float64 value2
    )
    {
        .maxstack 3
    
        ldarg value1
        ldarg value2
        ldarg operation
        switch (PLUS, MINUS, MULTIPLY, DIVIDE, UNDEFINED)
        
        PLUS:
            add
            br.s RESULT
        MINUS:
            sub
            br.s RESULT
        MULTIPLY:
            mul
            br.s RESULT
        DIVIDE:
            div
            br.s RESULT
        UNDEFINED: 
            pop
            pop
            newobj instance void [System.Runtime]System.InvalidOperationException::.ctor()
            throw
        
        RESULT: ret
    }
}




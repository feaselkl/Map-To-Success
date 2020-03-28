module MapToSuccess.Basics

// This is a single-line comment.
(* This is
a multi-line
comment. *)

// Mouse over outputText for details on the function.
// input:string -> unit.  Takes input as a string and outputs "unit"
// Unit is special and represents a shapeless, nameless form.
    // Kind of like NULL but not insidious.
let outputText input =
    printfn "This is a function which wants you to know the following:  %s " input

let addTwoNumbers x y =
    x + y
    // Notice we don't need a "return" statement.
    // Or an output type.
    // Or input types.  Those are inferred based on what you call.

// F# is whitespace-significant, though not as pushy as Python.
// Get rid of the curly braces and most parentheses...but not all of them!
// Note the function here:  x:int -> y:int -> int. This is an example of
// partial application, a really cool technique for working with functions.
// You'll see why this is useful in the Movies demo.
let performComplexOperations x y =
    let summation = x + y
    if summation > 50 then printfn "More than 50"
    else printfn "Not quite 50"

    // I'm still in the same function.
    (x * y) + summation

// F# is compatible with C# and VB.Net methods.
// This means you can load your C# libraries with ease.
let callCSharpMethod (x:string) =
    let hasFred = x.ToUpper().Contains("FRED")
    hasFred

// This is a type.  It's kind of like a struct.
// If you're familiar with classes, think of a class with no methods.
type MyType = {
    Val1:string
    Val2:string
    Val3:int
}

// We can use types as inputs to functions or outputs.
// F# has great type inference, so I don't need to tell it that mt is of type MyType.
let printAType mt =
    printfn "My type is Val1 = %s, Val2 = %s, Val3 = %i" mt.Val1 mt.Val2 mt.Val3

// Similarly, I can build a type implicitly.
// Semi-colons are generally not important in F#.
// This is an exception because we're packing multiple statements onto one line.
let makeAType val1 val2 val3 =
    { Val1 = val1; Val2 = val2; Val3 = val3 }

// Run these to test it out:
let myType = makeAType "Something" "Something Else" 919
printAType myType

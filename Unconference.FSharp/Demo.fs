module Demo

open NUnit.Framework
open FsUnit

type OrType = Option1 | Option2 //cardinality 2 => 1 + 1
type AndType = OrType * bool //cardinality 4 => 2 * 2
type ComplexStruc = { Arg1: OrType; Arg2: bool } //cardinality 4 => 2 * 2

[<Test>]
let equality () =
    { Arg1 = Option1; Arg2 = true } |> should equal { Arg1 = Option1; Arg2 = true }
module TennisKata

open NUnit.Framework
open FsUnit

type Player = Player1 | Player2 // cardinality : 2
type Point = Zero | Fifteen | Thirty  // cardinality : 3
type Game = // cardinality : 3 * 3 + 2 * 3 + 2 + 1 = 18
    | Score of Point * Point
    | Forty of Player * Point
    | Advantage of Player
    | Deuce

let incrementPointPlayer score = // cardinality : 3
    match score with 
    | Zero -> Fifteen
    | Fifteen -> Thirty
    | Thirty -> failwith "Exception"  

let incrementPoint game winner = // cardinality : 18 * 2 = 36
    match winner, game with
    | Player1, Deuce -> Advantage Player1
    | Player2, Deuce -> Advantage Player2
    | Player1, Advantage Player1 -> Score (Zero, Zero)
    | Player2, Advantage Player2 -> Score (Zero, Zero)
    | Player1, Advantage Player2 -> Deuce
    | Player2, Advantage Player1 -> Deuce
    | Player1, Forty (Player1, _)  -> Score (Zero, Zero)
    | Player2, Forty (Player2, _)  -> Score (Zero, Zero)
    | Player1, Forty (Player2, _)  -> Deuce
    | Player2, Forty (Player1, _)  -> Deuce
    | Player1, Score (Thirty, scoreP2) -> Forty (Player1, scoreP2)
    | Player2, Score (scoreP1, Thirty) -> Forty(Player2, scoreP1)
    | Player1, Score (scoreP1, scoreP2) -> Score (incrementPointPlayer scoreP1, scoreP2)
    | Player2, Score (scoreP1, scoreP2) -> Score (scoreP1, incrementPointPlayer scoreP2)






let incrementsPlayer1 = [|
        [| Score (Zero, Zero); Score (Fifteen, Zero) |]
        [| Score (Fifteen, Zero); Score (Thirty, Zero) |]
        [| Score (Thirty, Zero); Forty (Player1, Zero) |]
        [| Forty (Player1, Zero); Score (Zero, Zero) |]
        [| Forty (Player1, Fifteen); Score (Zero, Zero) |]
        [| Forty (Player2, Thirty); Deuce |]
        [| Deuce ; Advantage Player1 |]
        [| Advantage Player1; Score (Zero, Zero) |]
        [| Advantage Player2; Deuce |]
    |] 

[<Test>]
[<TestCaseSource "incrementsPlayer1">]
let ``Should increment player 1 point`` (initialScore, newScore) =
    incrementPoint initialScore Player1 |> should equal newScore
  
let incrementsPlayer2 = [|
        [| Score (Zero, Zero); Score (Zero, Fifteen) |]
        [| Score (Zero,Fifteen); Score (Zero,Thirty) |]
        [| Score (Zero,Thirty); Forty (Player2, Zero) |]
        [| Forty (Player2, Zero) ; Score (Zero, Zero) |]
        [| Forty (Player2, Fifteen) ; Score (Zero, Zero) |]
        [| Forty (Player1, Thirty); Deuce |]
        [| Deuce; Advantage Player2 |]
        [| Advantage Player1; Deuce |]
        [| Advantage Player2; Score (Zero, Zero) |]
|] 

[<Test>]
[<TestCaseSource "incrementsPlayer2">]
let ``Should increment player 2 point`` (initialScore, newScore) =
    incrementPoint initialScore Player2 |> should equal newScore
    
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
    

let incrementPlayer1PointForRegularScore (scoreP1, scoreP2) = // cardinality : 3 * 3 = 9
    match scoreP1 with
    | Zero -> Score (Fifteen, scoreP2)
    | Fifteen -> Score (Thirty, scoreP2)
    | Thirty -> Forty (Player1, scoreP2)

let incrementPlayer2PointForRegularScore (scoreP1, scoreP2) = // cardinality : 3 * 3 = 9
    match scoreP2 with
    | Zero -> Score (scoreP1, Fifteen)
    | Fifteen -> Score (scoreP1, Thirty)
    | Thirty -> Forty (Player2, scoreP1)

let incrementPointForRegularScore score = function // cardinality : 3 * 3 * 2 = 18
    | Player1 -> incrementPlayer1PointForRegularScore score
    | Player2 -> incrementPlayer2PointForRegularScore score 

let incrementFortyPointForOpponent playerWithForty = function // cardinality : 2 * 3 = 6
    | Zero -> Forty (playerWithForty, Fifteen)
    | Fifteen -> Forty (playerWithForty, Thirty)
    | Thirty -> Deuce

let incrementFortyPoint playerWithForty scoreOfOpponent winner = // cardinality : 2 * 3 * 2 = 12
    if playerWithForty = winner 
    then Score (Zero, Zero)
    else incrementFortyPointForOpponent playerWithForty scoreOfOpponent
    
let incrementAdvantagePoint playerAboutToWin winner = // cardinality : 2 * 2 = 4
    if playerAboutToWin = winner 
    then Score (Zero, Zero)
    else Deuce

let incrementPoint game = // cardinality : 18
    match game with
    | Deuce -> Advantage
    | Advantage playerAboutToWin -> incrementAdvantagePoint playerAboutToWin
    | Forty (playerWithForty, scoreOfOpponent) -> incrementFortyPoint playerWithForty scoreOfOpponent
    | Score (scoreP1, scoreP2) -> incrementPointForRegularScore (scoreP1, scoreP2) 
    


let incrementsPlayer1 = [|
        [| Score (Zero, Zero); Score (Fifteen, Zero) |]
        [| Score (Fifteen, Fifteen); Score (Thirty, Fifteen) |]
        [| Score (Thirty, Thirty); Forty (Player1, Thirty) |]
        [| Forty (Player1, Zero); Score (Zero, Zero) |]
        [| Forty (Player1, Fifteen); Score (Zero, Zero) |]
        [| Forty (Player2, Zero); Forty (Player2, Fifteen) |]
        [| Forty (Player2, Fifteen); Forty (Player2, Thirty) |]
        [| Forty (Player2, Thirty); Deuce |]
        [| Deuce; Advantage Player1 |]
        [| Advantage Player1; Score (Zero, Zero) |]
        [| Advantage Player2; Deuce |]
    |] 

[<Test>]
[<TestCaseSource "incrementsPlayer1">]
let ``Should increment player 1 point`` (initialScore, newScore) =
    incrementPoint initialScore Player1 |> should equal newScore
  
let incrementsPlayer2 = [|
        [| Score (Zero, Zero); Score (Zero, Fifteen) |]
        [| Score (Fifteen, Fifteen); Score (Fifteen, Thirty) |]
        [| Score (Zero, Thirty); Forty (Player2, Zero) |]
        [| Forty (Player2, Zero); Score (Zero, Zero) |]
        [| Forty (Player2, Fifteen); Score (Zero, Zero) |]
        [| Forty (Player1, Zero); Forty (Player1, Fifteen) |]
        [| Forty (Player1, Fifteen); Forty (Player1, Thirty) |]
        [| Forty (Player1, Thirty); Deuce |]
        [| Deuce; Advantage Player2 |]
        [| Advantage Player1; Deuce |]
        [| Advantage Player2; Score (Zero, Zero) |]
|] 

[<Test>]
[<TestCaseSource "incrementsPlayer2">]
let ``Should increment player 2 point`` (initialScore, newScore) =
    incrementPoint initialScore Player2 |> should equal newScore
    
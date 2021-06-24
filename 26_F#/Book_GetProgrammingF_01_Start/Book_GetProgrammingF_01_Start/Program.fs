// Define module MyCode in namespace Company.Rules (lesson 12)
module Company.Rules.MyCode
// Open System namespace
open System
// Define a simple value (lesson 4)
let playerName = "Joe"
// Create and unwrap a tuple (lesson 9)
let playerTuple = playerName, 21
let name, age = playerTuple
// Define and create a record (lesson 10)
type Player = { Name : string; Score : int; Country : string }
let player = { Name = playerName; Score = 0; Country = "GB" }
// Function definition with copy-and-update record syntax (lessons 10, 11)
let increaseScoreBy score p = { p with Score = p.Score + score }
// Piping functions (lesson 11)
player |> increaseScoreBy 50 |> printfn "%A"
// Function with basic pattern matching and nested expressions (lesson 7, 20)
type GreetingStyle = Friendly | Normal
let greet style player =
    let greeting =
        match style with
        | Friendly -> "Have a nice day!"
        | Normal -> "Good luck."
    sprintf "Hello, player %s! %s" player.Name greeting
// Partial function application (lesson 11)
let friendlyGreeting = greet Friendly
// Composing functions together (lesson 11)
let printToConsole text = printfn "%s" text
let greetAndPrint = friendlyGreeting >> printToConsole

[<EntryPoint>]
let main args =
    let res = (AdventFS.Day1.solve "./Day1/input.txt")
    printfn "solution is %s" (string res)
    0
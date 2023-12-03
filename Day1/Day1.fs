
namespace AdventFS

module Day1 =
    let solve filePath =
        System.IO.File.ReadLines(filePath)
        |> Seq.map(fun line -> Seq.filter System.Char.IsDigit line)
        |> Seq.map(fun digits -> 
            let first = Seq.tryHead digits
            let last = Seq.tryLast digits
            (first, last))
        |> Seq.choose(fun (first, last) -> 
            match (first, last) with
            | (Some f, Some l) -> Some (f, l)
            | _ -> None)
        |> Seq.map(fun (f, l) -> string f + string l)
        |> Seq.map(fun num -> int num)
        |> Seq.sum


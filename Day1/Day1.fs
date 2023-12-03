
namespace AdventFS

module Day1 =
    let parseString str: string = 
        let replacements: (string * string) list= [
            ("zero", "0");
            ("one", "1");
            ("two", "2");
            ("three", "3");
            ("four", "4");
            ("five", "5");
            ("six", "6");
            ("seven", "7");
            ("eight", "8");
            ("nine", "9")
            ]
        replacements
        |> Seq.fold(fun acc (word, num) -> acc.Replace(word, num)) str

    let solve filePath =
        System.IO.File.ReadLines(filePath)
        |> Seq.map(parseString)
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


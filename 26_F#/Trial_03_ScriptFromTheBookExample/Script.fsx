/// Podział łańcucha znaków na słowa w miejscach występowania spacji
let splitAtSpaces (text: string) =
 text.Split ' '
 |> Array.toList
/// Analizowanie łańcucha znaków pod kątem powtarzających się słów
let wordCount text =
 let words = splitAtSpaces text
 let numWords = words.Length
 let distinctWords = List.distinct words
 let numDups = numWords - distinctWords.Length
 (numWords, numDups)
/// Analizowanie łańcucha znaków pod kątem powtarzających się słów i wyświetlanie wyników
let showWordCount text =
 let numWords, numDups = wordCount text
 printfn "--> Liczba słów w tekście: %d" numWords
 printfn "--> Liczba powtórzeń: %d" numDups
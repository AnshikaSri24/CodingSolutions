using System;
using System.Collections.Generic;
using System.Linq;

namespace TestAppConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string input = "[1, 2, 1, 1, 1] | [2, 2, 2, 3, 3] | [5, 2, 6, 1, 4]";
            int numofPlayer = 3;
            input = input.Replace(" ", "");
            string[] input1 = input.Split('|');

            List<Kind> kindOutput = new();
            Dictionary<int, Kind> dictOutput = new();
            int playerCount = 1;
            foreach (var data in input1)
            {
                var newData = data.Trim('[').Trim(']').Split(',').Select(a => int.Parse(a)).ToList();
                var respose = CheckKind(newData);
                kindOutput.Add(respose);
                dictOutput.Add(playerCount++, respose);
            }

            string result = string.Empty;
            int player = 0;
            Kind kindResult = Kind.None;
            bool isHighest = false;
            if (kindOutput.Distinct().Count() == 3)
            {
                kindResult = kindOutput.Max();
                player = kindOutput.IndexOf(kindOutput.Max()) + 1;
            }
            else if (kindOutput.Distinct().Count() == 2)
            {
                kindResult = kindOutput.Max();
                //check if the max is twice or once
                if (kindOutput.Where(a => a == kindResult).Count() == 2)
                {
                    isHighest = true;
                }
            }
            else  //all players kind are same
            {
                kindResult = kindOutput.FirstOrDefault();
                if (kindResult == Kind.None)
                {
                    isHighest = true;
                }
            }
            List<int> listHighest = new();
            if (isHighest)
            {
                int counter = 0;
                foreach (var data in input1)
                {
                    var newData = data.Trim('[').Trim(']').Split(',').Select(a => int.Parse(a)).ToList();
                    //sum
                    int sum = 0;
                    for (int i = 0; i < newData.Count; i++)
                    {
                        sum += newData[i];
                    }
                    if (kindOutput[counter] == Kind.None)
                    {
                        sum = 0;
                    }
                    listHighest.Add(sum);
                    counter++;
                }

                player = listHighest.IndexOf(listHighest.Max()) + 1;
                dictOutput.TryGetValue(player, out kindResult);
            }

            switch (kindResult)
            {
                case Kind.TwoPair: result = "2"; break;
                case Kind.Straight: result = "Straight"; break;
                case Kind.FullHouse: result = "Full House"; break;
                case Kind.FourKind: result = "4"; break;
                case Kind.FiveKind: result = "5"; break;
            }

            Console.WriteLine($"Winner is player {player}  - {result} of a kind");
            Console.ReadKey();

        }
        public static Kind CheckKind(List<int> inputArray)
        {
            var distinct = inputArray.Distinct().ToList();
            if (distinct.Count == 1)
            {
                //5 of a kind
                return Kind.FiveKind;
            }
            else if (distinct.Count == 2)
            {
                string ratio = string.Empty;

                foreach (var uniqueData in distinct)
                {
                    //4 of a kind
                    if (inputArray.Where(a => a == uniqueData).Count() == 1 || inputArray.Where(a => a == uniqueData).Count() == 4)
                    {
                        ratio += inputArray.Where(a => a == uniqueData).Count().ToString();
                    }
                    //full house
                    else if (inputArray.Where(a => a == uniqueData).Count() == 3 || inputArray.Where(a => a == uniqueData).Count() == 2)
                    {
                        ratio += inputArray.Where(a => a == uniqueData).Count().ToString();
                    }
                }
                if (ratio == "14" || ratio == "41")
                {
                    return Kind.FourKind;
                }
                else if (ratio == "32" || ratio == "23")
                {
                    return Kind.FullHouse;
                }
            }
            else if (distinct.Count == 3)
            {
                //Full house
                int twoPairCount = 0;
                foreach (var uniqueData in distinct)
                {
                    if (inputArray.Where(a => a == uniqueData).Count() == 2)
                    {
                        twoPairCount++;
                    }
                }
                if (twoPairCount == 2)
                {
                    return Kind.TwoPair;
                }
            }
            else if (distinct.Count == 5)
            {
                //Sequence
                if (inputArray.Zip(inputArray.Skip(1), (a, b) => b - a).All(diff => diff == 1))
                {
                    return Kind.Straight;
                }
            }
            return Kind.None;
        }

    }
    public enum Kind
    {
        None,
        TwoPair,
        Straight,
        FullHouse,
        FourKind,
        FiveKind

    }
}

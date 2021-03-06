﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RomanNumerals
{

public static class Extensions
    {
        private enum PlaceValue : uint
        {
            Units = 1,
            Tens = 10,
            Hundreds = 100,
            Thousands = 1000
        }

        private static readonly PlaceValue[] placeValuesFomLeftToRight = 
            { PlaceValue.Thousands, PlaceValue.Hundreds, PlaceValue.Tens, PlaceValue.Units };

        private class SymbolsForPlaceValue
        {
            private static readonly char[] symbols = { 'I', 'V', 'X', 'L', 'C', 'D', 'M' };
            private static readonly Dictionary<PlaceValue, int> baseSymbolIndexForPlaceValue = 
                new Dictionary<PlaceValue, int>() 
                {
                    { PlaceValue.Units,     0 },
                    { PlaceValue.Tens,      2 },
                    { PlaceValue.Hundreds,  4 },
                    { PlaceValue.Thousands, 6 }
                };

            private int baseSymbolIndex;

            public SymbolsForPlaceValue(PlaceValue placeValue) =>
                baseSymbolIndex = baseSymbolIndexForPlaceValue[placeValue];

            public char Base() => symbols[baseSymbolIndex];
            public char BaseTimesFive() => symbols[baseSymbolIndex + 1];
            public char BaseTimesTen() => symbols[baseSymbolIndex + 2];
        }

        public static string ToRomanNumeralString(this int number)
        {
            if (number < 0 || number > 3999)
                throw new ArgumentOutOfRangeException("Argument must be between 0 and 3999", "number");

            return ((uint)number).ToRomanNumeralStringNoCheck();
        }

        public static string ToRomanNumeralString(this uint number)
        {
            if (number > 3999)
                throw new ArgumentOutOfRangeException("Argument must be less than or equal to 3999", "number");

            return number.ToRomanNumeralStringNoCheck();
        }

        private static string ToRomanNumeralStringNoCheck(this uint number)
        {
            String romanNumerals = "";

            foreach (PlaceValue placeValue in placeValuesFomLeftToRight)
                romanNumerals += number.DigitAtPlaceValueToRomanNumeralString(placeValue);

            return romanNumerals;
        }

        private static string DigitAtPlaceValueToRomanNumeralString(this uint number, PlaceValue placeValue)
        {
            uint digit = number.DigitAtPlaceValue(placeValue);

            switch (digit)
            {
                case 9:
                {
                    var s = new SymbolsForPlaceValue(placeValue);
                    return s.Base().ToString() + s.BaseTimesTen();
                }

                case 4:
                {
                    var s = new SymbolsForPlaceValue(placeValue);
                    return s.Base().ToString() + s.BaseTimesFive();
                }

                case 0:
                    return "";
            }

            var symbols = new SymbolsForPlaceValue(placeValue);
            String romanNumerals = "";

            if (digit >= 5)
            {
                romanNumerals += symbols.BaseTimesFive();
                digit -= 5;
            }

            while (digit >= 1)
            {
                romanNumerals += symbols.Base();
                digit -= 1;
            }

            return romanNumerals;
        }

        private static uint DigitAtPlaceValue(this uint number, PlaceValue placeValue) => 
            number / (uint)placeValue % 10;
    }
}

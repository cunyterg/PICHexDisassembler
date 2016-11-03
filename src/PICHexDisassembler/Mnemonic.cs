﻿using PICHexDisassembler.Instructions;
using System;
using System.Collections.Generic;

namespace PICHexDisassembler
{
    internal class Mnemonic
    {
        private static MnemonicMapping mnemonicMapping = new MnemonicMapping
        {
            { 0x2800, 0xF800, typeof(Goto) },   // mask: 0010100000000000 opcodeMask: 1111100000000000
            { 0x2000, 0xF800, typeof(Call) },   // mask: 0010000000000000 opcodeMask: 1111100000000000
            { 0x0009, 0xFFFF, typeof(Retfie) }, // mask: 0000000000001001 opcodeMask: 1111111111111111
            { 0x1400, 0xFC00, typeof(Bsf) },    // mask: 0001010000000000 opcodeMask: 1111110000000000
            { 0x1000, 0xFC00, typeof(Bcf) },    // mask: 0001000000000000 opcodeMask: 1111110000000000
        };

        internal static Instruction Parse(byte[] dataBytes)
        {
            var data1 = dataBytes[0];
            var data2 = dataBytes[1];
            var word = (short)(data1 << 8 | data2);

            foreach (var item in mnemonicMapping)
            {
                if ((word & item.Item2) == item.Item1)
                {
                    return (Instruction)Activator.CreateInstance(item.Item3, word);
                }
            }

            return Unknown.Instance;
        }
    }
}

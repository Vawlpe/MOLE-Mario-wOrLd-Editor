﻿using System;
using System.Linq;

namespace MOLE
{
    public class GFX
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public uint[] GFXPointers = new uint[0x34];
        public uint[] ExGFXPointers = new uint[0x80];
        public uint[] SuperExGFXPointers = new uint[0xF00];

        public byte[][] dGFX = new byte[0x34][];
        public GFX(ROM rom)
        {
            var Low = rom.Skip(rom.SnesToPc(0x00B992)).Take(0x32).ToArray();
            var High = rom.Skip(rom.SnesToPc(0x00B9C4)).Take(0x32).ToArray();
            var Bank = rom.Skip(rom.SnesToPc(0x00B9F6)).Take(0x32).ToArray();
            Logger.Debug("Getting GFX Pointers...");
            for (int i = 0; i < 0x32; i++)
            {
                GFXPointers[i] = BitConverter.ToUInt32(new byte[] { Low[i], High[i], Bank[i], 0});
            }
            GFXPointers[0x32] = 0x088000;
            GFXPointers[0x33] = 0x08BFC0;

            Logger.Debug("Getting ExGFX Pointers...");
            var ex = rom.Skip(rom.SnesToPc(0x0FF600)).Take(0x180).ToArray();
            for (int i = 0; i < 0x80; i++)
            {
                ExGFXPointers[i] = Util.b2uL(ex.Skip(i * 3).Take(3).ToArray());
            }

            if (rom.ROMSize <= 512)
            {
                Logger.Warn("Unexpanded ROM, SuperExGFX can't be used");
                Array.Fill<uint>(SuperExGFXPointers, 0xFFFFFF);
            }
            else
            {
                Logger.Debug("Getting SuperExGFX Pointers...");
                var supex = rom.Skip(rom.SnesToPc((int)Util.b2uL(rom.Skip(rom.SnesToPc(0x0FF937)).Take(3).ToArray()))).Take(0x2D00).ToArray();
                for (int i = 0; i < 0xF00; i++)
                {
                    SuperExGFXPointers[i] = Util.b2uL(supex.Skip(i * 3).Take(3).ToArray());
                }
            }

            Logger.Debug("Decompressing GFX");
            var lz2 = new Smallhacker.TerraCompress.Lz2();
            for (int i = 0; i < GFXPointers.Length; i++)
            {
                dGFX[i] = lz2.Decompress(rom.ToArray(), (uint)rom.SnesToPc((int)GFXPointers[i]));

                Logger.Debug("  GFX{0:X2} size: {1} bytes", i, dGFX[i].Length);
            }
            
        }
    }
}

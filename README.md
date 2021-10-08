**MOLE** is being designed as an **OpenSource**  editor for **Super Mario World** with the intent to bring many of the features from existing SMW ROMHacking tools/patches such as **Lunar Magic**, **Asar**, **YYCHR**, and [many others](#compatibility) into a single easy-to-use [**multi-platform**](#how-to-run) and [localized](#localizationtranslation) tool.
___
### How to Run
At the moment MOLE is still in a very early stage and therefore no public releases have been made
- Windows `(WIP)`
- Linux `(WIP)`
- MacOS (OSX) `(WIP)`

Or you could build it yourself:
___
### How to Build
	(SECTION WIP)
___
### Localization/Translation
	(SECTION WIP)
___
### Project Structure
```
•─ buildSystem/
	•─ Automatic build pipelines
•─ res/
	•─ Icons, Images,  Text KeyTables (for localization), etc...
•─ src/
	•─ MOLE/
		•─ Backend code, most systems rely on this to edit the ROM.
	•─ UI/
		•─ ImGui UI window definitions and main program entry point.
	•─ ImGUI.Net/
		•─ ImGui.Net library submodule.
	•─ VeldridController/
		•─ Veldrid UI Rendering backend for DirectX 11, Vulka, and Metal (OSX) support (with more targets coming soon)
	•─ XNAController/
		•─ Monogame XNA based UI Rendering backend for OpenGL support (with more targets coming soon)
```
___
### Compatibility
(WIP)
MOLE intends to be compatible with both clean ROMs, aswell as most modern SMW ROMhacking tools, ROM variants, and patches. This includes:
#### ROM Versions:
- North American (NTSC U)
- SA-1 (NTSC U only, using the SA-1 Kit patch)
- Japanese (NTSC J)<sup>Only basic compatibility, see [#20](/../../issues/20)</sup>
- All Stars + World<sup>Only basic compatibility, see [#21](/../../issues/20)</sup>
#### Tools:
|  | To MOLE | From MOLE | File formats |
|:---:|:---:|:---:|:---:|
| Lunar Magic | WIP<br>⚠️Partial⚠️<br>Currently our efforts are focused on clean ROMs, therefore certain LM specific features may be incompatible | WIP<br>⚠️Partial<br>LM will force a copier header<br>Some MOLE specific hijacks may be incompatible | ROM's (.smc, .sfc), Level (.mwl), Map16 (.map16), Graphics (.bin), Palette (.pal) |
| Asar | ✅Full✅ | ⚠️Partial⚠️<br>Asar will refuse to work .sfc files with a copier header, and .smc files without one, for consistency | ROM (.smc, .sfc), Patch (.asm) |
| YY-CHR (And other SNES tile editors) | WIP<br>✅Full✅ | WIP<br>✅Full✅ | Graphics (.bin), Palette (.pal) |
| AddMusicK | WIP<br>✅Full✅, planned for future release, currently ❓unimplemented❓ | WIP<br>⚠️Partial⚠️<br>Most tools expect the ROM to have a copier header and a .smc file extension | ROM (.smc, .sfc), Music (.txt), Sample (.brr), Config info (Addmusic_sample groups.txt, Addmusic_list.txt)|
| PIXI | WIP<br>✅Full✅, planned for future release, currently ❓unimplemented❓ | WIP<br>⚠️Partial⚠️<br>Most tools expect the ROM to have a copier header and a .smc file extension | ROM (.smc, .sfc), Sprite code (.asm), Config info (.cfg, .json) |
| GPS | WIP<br>✅Full✅, planned for future release, currently ❓unimplemented❓ | WIP<br>⚠️Partial⚠️<br>Most tools expect the ROM to have a copier header and a .smc file extension | ROM (.smc, .sfc), Block code (.asm, routines/.asm), Config info (list.txt) |
| UberASM | WIP<br>✅Full✅, planned for future release, currently ❓unimplemented❓ | WIP<br>⚠️Partial⚠️<br>Most tools expect the ROM to have a copier header and a .smc file extension | ROM (.smc, .sfc), Patch (.asm), Config info (list.txt) |
___
### More information on SMW ROMHacking and MOLE
You can find a lot of useful information about MOLE, SMW ROMhacking, and SNES development in general, in the [Useful Links](../../wiki/useful-links) Section of the [MOLE Wiki](../../wiki)
___
### License
[GNU General Public License v3.0](/LICENSE.md)

MOLE is an open source Super Mario world ROM editor and is in no way affiliated with Nintendo.
Copyright (C) 2021 Vawlpe

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.

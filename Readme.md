# Command Line Tool For Manipulating Colors
This is tool that helps you generate palettes with darker/lighter colors.

# Quick Start example
- Display help by executing exe: `HexColorHelperCli.exe`
- Make palette of darker colors: `HexColorHelperCli.exe hover -d -c #e60049,#0bb4ff,#50e991,#e6d800,#9b19f5,#ffa300,#dc0ab4,#b3d4ff,#00bfa0 -s 0.7`
- Make palette of darker colors, generate png file and open it: `HexColorHelperCli.exe hover -d -c #e60049,#0bb4ff,#50e991,#e6d800,#9b19f5,#ffa300,#dc0ab4,#b3d4ff,#00bfa0 -f palette -s 0.7`.
Will open file `palette.png` (from -f option) that will look like:
![alt text](darkerPalette.png)
- Make palette of lighter colors, generate png file and open it: `HexColorHelperCli.exe hover -c #e60049,#0bb4ff,#50e991,#e6d800,#9b19f5,#ffa300,#dc0ab4,#b3d4ff,#00bfa0 -f palette -s 0.6`.
Will open file `palette.png` (from -f option) that will look like:
![alt text](lighterPalette.png)
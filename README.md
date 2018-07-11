# SwitchGameManager

Manage your XCI files on SD and PC, from multiple PC folders. Rename, search, trim, sort, copy/move/delete games.
Display your library in a few different ways, with more likely to come. For now, it relies on hacbuild to extract the PackageID of each XCI and uses that as a unique identifier. I don't recommend having the same game multiple times in your library as that hasn't been well tested. I'll transition to [libhac soon.](https://github.com/Thealexbarney/libhac)

## Important
You need a keys.txt file to use this program.
In addition to the usual keys, you'll also need **xci_header_key**!

#### Screenshots
![Screenshot](Screens/1.png)
![Screenshot](Screens/2.png)
![Screenshot](Screens/3.png)
![Screenshot](Screens/4.png)

## Bugs
- Crashes if any file operations (such as trimming) occur during a file transfer
- It's slow to build the library initially. I'll move to libhac soon.
- Can copy the same file(s) multiple times
- Probably some others, please report them!
- Doesn't verify keys.txt existence

## Current todo list
- [ ] Make a todo list

### Credits
http://objectlistview.sourceforge.net/cs/index.html
https://stackoverflow.com/a/6055385
https://github.com/LucaFraga/hacbuild
xci_explorer decompiled and https://www.nuget.org/packages/Be.Windows.Forms.HexBox/
Newtonsoft.Json
https://www.iconfinder.com/icons/3151574/game_nintendo_switch_video_icon
Lots of people on StackExchange/Overflow

And probably a few I'm missing.

### Donations
I worked a lot on this, but there is no fee for using it! If you appreciate my efforts, you can send a donation to any of these addresses.

 * BTC: 1QDVJmxyqMzA5nQghKMBCFVk8K41nSoz5b
 * ETH: 0xa62a11710cc44Bd54D66CbCcF710a36716BF04CE
 * Monero: 43tVLRGvcaadfw4HrkUcpEKmZd9Y841rGKvsLZW8XvEVSBX1GrGezWvQYDdoNwNHAwTqSyK7iqyyqMSpDoUVKQmM43nzT72
 * UBQ: 0x0c0ff71b06413865fe9fE9a4C40396c136a62980
 * DCR: DsfPh3tpa7nd8sExYvxWbijzjUH1zJ34dgu
 * HUSH: t1ZHrvmtgd3129iYEcFm21XMv5ojdh2xmsf
 * ZEN: znTmG8nid2LEYgw8cub17Q7briGATan4c68
 

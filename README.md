# HAMToChat
[![Static Badge](https://img.shields.io/badge/HAMToChat-v0.7-green)](https://github.com/1sup4ik1/HAMToChat/releases/tag/v0.7)
## In Game

<img src="https://github.com/1sup4ik1/HAMToChat/blob/master/image/image.png" alt="login">

## Features
- Your Heart Beat
- Your active application
- Your media player
- - Name of music
- - Music lyrics
- - Name of artist
- - Music playing time
## Getting started
#### If You use Pulsoid
1. Registry on [Pulsoid.net](https://pulsoid.net/)
2. Go to [Pulsoid Configuration](https://pulsoid.net/ui/configuration)
3. Scroll to bottom of the webside, find "Feed reference" and copy it
4. Start the HAMToChat.exe
5. After "config.json" is created close the HAMToChat
6. Open the "config.json" and replace "NULL" in ApiHB with "pulsoid"
7. In "config.json" replace "NULL" in Token with your link. Exemple: "https://pulsoid.net/v1/api/feed/YOUR_FEED_REFERENCE"
8. Done!
#### You also can use direct bluetooth connection
1. Open the "config.json" and replace "NULL" in ApiHB with "vardAPI"(if you are using miband 7 use "Ismb7")
2. Follow the instructions from this [GitRep](https://github.com/vard88508/vrc-osc-miband-hrm)
3. Done!
#### Console activate
- To enable console put "-console" to args
## Credits
- [Vard88508](https://github.com/vard88508) for [vrc-osc-miband-hr](https://vard88508.github.io/vrc-osc-miband-hrm/html/)
- [patyork](https://github.com/patyork) for [miband-7-monitor](https://github.com/patyork/miband-7-monitor/)
- [DudyaDude](https://github.com/DubyaDude) for [WindowsMediaController](https://github.com/DubyaDude/WindowsMediaController)

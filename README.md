# HAMToChat
[![Static Badge](https://img.shields.io/badge/HAMToChat-v1-green)](https://github.com/1sup4ik1/HAMToChat/releases/tag/v1)
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
#### If You use Pulsoid(Supported devices [here](https://www.blog.pulsoid.net/monitors?from=mheader))
1. Registry on [Pulsoid.net](https://pulsoid.net/)
2. Go to [Pulsoid Configuration](https://pulsoid.net/ui/configuration)
3. Scroll to bottom of the webside, find "Feed reference" and copy it
4. Start the HAMToChat.exe
5. After "config.json" is created close the HAMToChat
6. Open the "config.json" and replace "NULL" in ApiHB with "pulsoid"
7. In "config.json" replace "NULL" in Token with your link. Exemple: "https://pulsoid.net/v1/api/feed/YOUR_FEED_REFERENCE"
8. Done!
#### You also can use direct bluetooth connection
1. Get auth key of your device. (https://mmk.pw/xiaomikey/ - Best option) (For more information - please visit https://freemyband.com/ or https://github.com/argrento/huami-token if you experienced with python)
2. Download and launch HAMToChat.exe
3. Open the "config.json" and replace "NULL" in ApiHB with "vardAPI"(if you are using miband 7 use "Ismb7")
4. Restart HAMToChat.exe
5. After opening a browser window enter your auth key and click Connect (Make sure you turned off bluetooth on your phone)
6. Done!
#### Console activate
- To enable console put "-console" to args
## Credits
- [Vard88508](https://github.com/vard88508) for [vrc-osc-miband-hr](https://github.com/vard88508/vrc-osc-miband-hrm/)
- [patyork](https://github.com/patyork) for [miband-7-monitor](https://github.com/patyork/miband-7-monitor/)
- [DudyaDude](https://github.com/DubyaDude) for [WindowsMediaController](https://github.com/DubyaDude/WindowsMediaController)

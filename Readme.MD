This script/repo/tool does follow the automation guidelines on the /r/adventofcode community wiki [https://www.reddit.com/r/adventofcode/wiki/faqs/automation] somewhere). Specifically:

Outbound calls are only run the first time a day's puzzle is executed
Once inputs are downloaded, they are cached locally AoC.Utils\Utils\IPuzzleExtensions.cs : GetInput()
If you suspect your input is corrupted, you can manually request a fresh copy by deleting the local copy and re-running
The User-Agent header in AoC.Utils\Util.cs : Download() set to me since I maintain this tool :)
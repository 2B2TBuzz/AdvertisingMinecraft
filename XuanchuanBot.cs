//MCCScript 1.0



MCC.LoadBot(new GreetNewPlayersBot());

//MCCScript Extensions


class GreetNewPlayersBot : ChatBot
{
    private System.Timers.Timer _timer;
    private List<string> _greetings = new List<string>
    {
        "2B2T.BUZZ 低版本无规则服务器 群号745522080",
        "2B2T.BUZZ 邀请别人可获得物资 群号745522080",
        "2B2T.BUZZ 超自由无反作弊 745522080",
        // 添加更多你想发送的消息
    };

    private Random _random = new Random();
    private bool _canSendMessage = false;

    public override void Initialize()
    {
        LogToConsole("Greet New Players Bot has been initialized!");
    }

    public override void AfterGameJoined()
    {
        // 注册
        SendText("/reg 114514 114514");
        SendText("/l 114514");
        PerformInventoryAction();
    }

    private void PerformInventoryAction()
    {
        // 模拟右键点击快捷栏第三个物品
        // 假设物品栏从0开始计数，第三个物品是索引2
        if (GetInventoryEnabled())
        {
            // 切换到第三个物品
            ChangeSlot(2);
            // 使用物品
            UseItemInHand();
            LogToConsole("Joined the Server");
            // 设置可以发送消息
            _canSendMessage = true;
        }
        else
        {
            LogToConsole("Inventory handling is not enabled.");
        }
    }

    private void StartGreetingTimer()
    {
        // 设置定时器，每10秒触发一次
        _timer = new System.Timers.Timer(10000);
        _timer.Elapsed += OnTimerEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private void OnTimerEvent(object source, System.Timers.ElapsedEventArgs e)
    {
        if (_canSendMessage)
        {
            // 获取当前在线玩家列表
            string[] onlinePlayers = GetOnlinePlayers();
            lock (_random)
            {
                foreach (string playerName in onlinePlayers)
                {
                    string message = _greetings[_random.Next(_greetings.Count)];
                    SendPrivateMessage(playerName, message);
                    LogToConsole("Sent message to " + playerName + ": " + message);
                }
            }
        }
    }

    public override void OnUnload()
    {
        if (_timer != null)
        {
            _timer.Stop();
            _timer.Dispose();
        }
        LogToConsole("Greet New Players Bot has been unloaded.");
    }
}

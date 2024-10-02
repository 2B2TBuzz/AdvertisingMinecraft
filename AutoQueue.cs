//MCCScript 1.0
using System;
using System.Collections.Generic;


MCC.LoadBot(new AutoResponderBot());

class AutoResponderBot : ChatBot
{
    private Dictionary<string, string> _questionMap = new Dictionary<string, string>()
    {
        { "红石火把", "15" },
        { "猪被闪电", "僵尸猪人" },
        { "小箱子能", "27" },
        { "开服年份", "2020" },
        { "定位末地遗迹", "0" },
        { "爬行者被闪电", "高压爬行者" },
        { "大箱子能", "54" },
        { "羊驼会主动", "不会" },
        { "无限水", "3" },
        { "挖掘速度最快", "金镐" },
        { "凋灵死后", "下界之星" },
        { "苦力怕的官方", "爬行者" },
        { "南瓜的生长", "不需要" },
        { "定位末地", "0" }
    };
    private Random _random = new Random();

    public override void Initialize()
    {
        LogToConsole("AutoResponder Bot has been initialized!");
    }

    public override void GetText(string text)
    {
        string verbatimText = GetVerbatim(text);
        string message = "";
        string username = "";

        if (IsChatMessage(verbatimText, ref message, ref username))
        {
            // 假设服务器端发送的问题格式为：问题？选项：A.答案1 B.答案2 C.答案3。
            string[] parts = message.Split(new string[] { "选项：" }, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                string questionPart = parts[0].Trim();
                string optionsPart = parts[1].Trim();

                // 提取问题关键词
                string keyword = ExtractKeyword(questionPart);
                if (!string.IsNullOrEmpty(keyword) && _questionMap.ContainsKey(keyword))
                {
                    // 获取答案对应的选项
                    string answer = _questionMap[keyword];
                    string reply = FindAnswerInOptions(optionsPart, answer);

                    if (!string.IsNullOrEmpty(reply))
                    {
                        SendText(reply);
                        LogToConsole("Auto-replied with: " + reply);
                    }
                }
            }
        }
    }

    private string ExtractKeyword(string question)
    {
        // 根据实际情况调整提取关键词的逻辑
        string[] keywords = question.Split(new char[] { '.', ' ', '?' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var key in keywords)
        {
            if (_questionMap.ContainsKey(key))
            {
                return key;
            }
        }
        return null;
    }

    private string FindAnswerInOptions(string options, string answer)
    {
        // 假设答案格式为：A.答案
        var matches = Regex.Matches(options, @"[A-C]\." + answer);
        if (matches.Count > 0)
        {
            // 返回匹配到的答案选项，例如：A
            return matches[0].Value.Replace(answer, "").Trim();
        }
        return null;
    }
}
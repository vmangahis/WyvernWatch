using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using WyvernWatch.Interfaces;


namespace WyvernWatch;

public class WyvernFunction
{
    private readonly IMailService mailService;


    public WyvernFunction(IMailService ms)
    {
        mailService = ms;
    }

    [Function("WyvernFunction")]
    public void Run([TimerTrigger("0 50 23 * * *")] TimerInfo myTimer)
    {
        mailService.SendEmail();
    }
}
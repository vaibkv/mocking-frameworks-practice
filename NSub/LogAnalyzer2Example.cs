using System;
using NSubstitute;
using NUnit.Framework;

namespace NSub
{
	public class LogAnalyzer2Example
	{
		//What you have to test//
		//When logAnalyzer's Analyze is called and if logger's logError throws an exception, then webService's write method
		//should be called

		//WITHOUT NSUBSTITUTE
		[Test]
		public void Analyze_LoggerThrows_CallsWebService()
		{
			FakeWebService mockWebService = new FakeWebService();
			FakeLogger2 stubLogger = new FakeLogger2();
			stubLogger.WillThrow = new Exception("fake exception");
   			var analyzer2 =
 			new LogAnalyzer2(stubLogger, mockWebService);
			analyzer2.MinNameLength = 8;
			string tooShortFileName = "abc.ext";
			analyzer2.Analyze(tooShortFileName);
			Assert.That(mockWebService.MessageToWebService, Is.StringContaining("fake exception"));
		}

		//WITH NSUBSTITUTE
		[Test]
		public void Analyze_LoggerThrows_CallsWebService_NSub()
		{
			//Arrange
			IWebService fakeWebService = Substitute.For<IWebService>();
			ILogger fakeLogger = Substitute.For<ILogger>();
			fakeLogger.When(x => x.LogError(Arg.Any<string>())).Do(info => { throw new Exception("some fake exception");});
			LogAnalyzer2 logAnalyzer = new LogAnalyzer2(fakeLogger, fakeWebService);
			logAnalyzer.MinNameLength = 20;

			//Act
			logAnalyzer.Analyze("short.txt");

			//Assert
			fakeWebService.Received().Write(Arg.Is<string>(s => s.Contains("some fake exception")));
		}
	}

	public class FakeWebService : IWebService
	{
		public string MessageToWebService;
		public void Write(string message)
		{
			MessageToWebService = message;
		}
	}

	public class FakeLogger2 : ILogger
	{
		public Exception WillThrow = null;
		public string LoggerGotMessage = null;

		public void LogError(string message)
		{
			LoggerGotMessage = message;
			if (WillThrow != null)
			{
				throw WillThrow;
			}
		}
	}

	//---------- PRODUCTION CODE------------//
	public class LogAnalyzer2
	{
		private ILogger _logger;
		private IWebService _webService;

   		public LogAnalyzer2(ILogger logger, IWebService webService)
		{
			_logger = logger;
			_webService = webService;
		}

		public int MinNameLength { get; set; }

		public void Analyze(string filename)
		{
			if (filename.Length < MinNameLength)
			{
				try
				{
					_logger.LogError(
					string.Format("Filename too short: {0}", filename));
				}
				catch (Exception e)
				{
					_webService.Write("Error From Logger: " + e);
				}
			}
		}
	}

	public interface IWebService
	{
		void Write(string message);
	}
}
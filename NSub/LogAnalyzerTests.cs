using System;
using NUnit.Framework;
using NSubstitute;

namespace NSub
{
	//Using manual fake logger
	/*[TestFixture]
	class LogAnalyzerTests
	{
    	[Test]
    	public void Analyze_TooShortFileName_CallLogger()
    	{
			FakeLogger logger = new FakeLogger();
			LogAnalyzer analyzer = new LogAnalyzer(logger);
			analyzer.MinNameLength= 6;
			analyzer.Analyze("a.txt");
			StringAssert.Contains("too short",logger.LastError);
   	} 	}

	class FakeLogger: ILogger
	{
    	public string LastError;
    	public void LogError(string message)
		{
        	LastError = message;
    	}
	}*/

	//Using NSubstitute to create fake logger
	[TestFixture]
	class LogAnalyzerTests
	{
		[Test]
		public void Analyze_TooShortFileName_CallLogger()
		{
			//making a fake
			ILogger logger = Substitute.For<ILogger>(); 

			//using that fake to instantiate a real object of our liking
			LogAnalyzer analyzer = new LogAnalyzer(logger);
			analyzer.MinNameLength = 6;
			analyzer.Analyze("a.txt");

			//basically asserting whether LogError was called with a message "too short". And since we asserted on the fake object, 
			//it seems we actually created a mock 
			logger.Received().LogError("too short");
		}
	}

	public interface ILogger
	{
		void LogError(string message);
	}

	public class LogAnalyzer
	{
		public int MinNameLength {get;set;}
		private ILogger logger;

		public LogAnalyzer(ILogger logger)
		{
			this.logger = logger;
		}

		public void Analyze(string fileName)
		{
			if (fileName.Length < MinNameLength)
			{
				logger.LogError("too short");
			}
		}
	}
}
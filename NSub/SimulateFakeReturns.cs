using System;
using NUnit.Framework;
using NSubstitute;

namespace NSub
{
	[TestFixture]
	public class SimulateFakeReturns
	{
		[Test]
		public void Returns_ByDefault_WorksForHardCodedArgument()
		{
			IFileNameRules fakeRules = Substitute.For<IFileNameRules>();

			//when this is called that is returned
			fakeRules.IsValidLogFileName("strict.txt").Returns(true);

			Assert.IsTrue(fakeRules.IsValidLogFileName("strict.txt"));
		}

		[Test]
		public void Returns_ByDefault_WorksForAnyArgument()
		{
			IFileNameRules fakeRules = Substitute.For<IFileNameRules>();

			//return this when input is _any_ string 
			fakeRules.IsValidLogFileName(Arg.Any<String>())
			.Returns(true);
			
			Assert.IsTrue(fakeRules.IsValidLogFileName("anything.txt"));
		}

		//how to tell fake to throw exception
		[Test]
		public void Returns_ArgAny_Throws()
		{
			IFileNameRules fakeRules = Substitute.For<IFileNameRules>();

			fakeRules.When(x => x.IsValidLogFileName(Arg.Any<string>()))
			         .Do(context =>    { throw new Exception("fake exception"); });
			
			Assert.Throws<Exception>(() =>
			   fakeRules.IsValidLogFileName("anything"));
		}

 	}

	public interface IFileNameRules
	{
		bool IsValidLogFileName(string fileName);
	}
}
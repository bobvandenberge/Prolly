# Prolly #

Really lightweight and simple Circuit Breaker / Timeout library for .NET. Inspired by Hystrix and Polly

## Usage ##

### Step 1 : Inherit an command from ProllyCommand&lt;T&gt; 
To create a prolly command, create a new class and let it inherit from ProllyCommand&lt;T&gt;. The T should be the return value. Currently void methods aren't supported. This is planned for the future.

When you create a new command you have to specify the command group it belongs to. All commands in a certain command group use the same circuit breaker. You can specify a command group by a key. If it doesn't exist yet, it will be automatically created.

    class DemoCommand : ProllyCommand<string>
    {
        public DemoCommand()
            :base("DemoCommandGroup")
        { }

        protected override string Run()
        {
            return CallToExternalSomething();
        }
    }


### Step 2 : Execute the command
After a new command is created you can execute it by calling the Execute() method. 

            var command = new DemoCommand();
            command.Execute();

### Step 3 (optional) : Specifying a fallback
It might be desirable to return a value instead of an exception when an call couldn't be executed properly. For this you can override the Fallback method inside your custom command.

    class DemoCommand : ProllyCommand<string>
    {
        public DemoCommand()
            : base("DemoCommandGroup")
        { }

        protected override string Run()
        {
            return CallToExternalSomething();
        }

        protected override string Fallback()
        {
            return "Some fallback data";
        }
    }

## Configuration ##

There are a few settings you can tweak. These are currently: the timeout time, the times a call is allowed to fail before the circuit breaker opens and the time the circuit breaker stays open.

### Step 1 : Define the configuration Section
Define the configuration section inside your App/Web.config

    <configSections>
      <sectionGroup name="prollySettings">
        <section 
          name="prolly" 
          type="Prolly.Configuration.Sections.ProllySection, Prolly" 
          allowLocation="true" 
          allowDefinition="Everywhere" />
      </sectionGroup>
    </configSections>

### Step 2 : Define your configuration
The configurations will automatically be applied.

    <prollySettings>
      <prolly>
        <timeout miliseconds="200"/>
        <circuitBreaker allowedFailures="2" openTimeInMiliseconds="200" />
      </prolly>
    </prollySettings>
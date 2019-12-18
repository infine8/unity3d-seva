#import "UnityTestFlightSettings.h"
#import "TestFlight.h"
#import "TestFlight+OpenFeedback.h"
// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {

	void _iOS_TestFlight_takeOff(const char* appToken)
	{
#if UNITY_TESTFLIGHT_USE_UDUD == 1
        if([[UIDevice currentDevice] respondsToSelector:@selector(uniqueIdentifier)])
        {
            NSString* udid = [[UIDevice currentDevice] performSelector:@selector(uniqueIdentifier)];
            [TestFlight setDeviceIdentifier:udid];
            NSLog(@"UDID: %@", udid);
        }
#endif
        [TestFlight takeOff: CreateNSString(appToken)];
	}
	
	void _iOS_TestFlight_log(const char* message)
	{
        TFLogPreFormatted(CreateNSString(message));
	}
    
    void _iOS_TestFlight_passCheckpoint(const char* checkpoint)
	{
        [TestFlight passCheckpoint: CreateNSString(checkpoint)];
	}
    
    void _iOS_TestFlight_submitFeedback(const char* feedback)
	{
        [TestFlight submitFeedback: CreateNSString(feedback)];
	}
    
    void _iOS_TestFlight_openFeedbackView()
	{
        [TestFlight openFeedbackView];
	}
    
    void _iOS_TestFlight_forceCrash()
	{
        NSLog(@"iOS.TestFlight.forceCrash: I will crash now!");
        //Fun fact
        //Divide-by-zero is illegal on i386 and x84-64, but is a valid operation on ARM!  Dividing by zero will cause crashes in the simulator, but not on iOS devices.
        //int fail = (int)1 / (int)0;
        int *x = NULL; *x = 42;
        //in case the app didn't crash
        assert(false);
	}
}


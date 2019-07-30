Add-Type -AssemblyName System.speech
$speak = New-Object System.Speech.Synthesis.SpeechSynthesizer
# $speak.Speak("This is a test of some PowerShell code")
$speak.Speak("good morning!")

$speak.GetInstalledVoices()|ForEach-Object { $_.VoiceInfo }
$voices = @('Microsoft David Desktop', 'Microsoft Zira Desktop', 'Microsoft Huihui Desktop')
$speak.SelectVoice($voices[1])
$speak.Speak("good morning!")

$test_code = @"
using System;
using System.Speech.Synthesis;

public class TestText2Speech {
    public static void Say(string[] args){
        var speak = new SpeechSynthesizer();
        // speak.Speak("good morning!");
        speak.Speak(string.Join(" ", args));
    }
}
"@
Add-Type $test_code -ReferencedAssemblies System.Speech
[TestText2Speech]::Say(@('hello', 'world'))

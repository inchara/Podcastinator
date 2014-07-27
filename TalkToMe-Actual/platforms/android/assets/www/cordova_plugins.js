cordova.define('cordova/plugin_list', function(require, exports, module) {
module.exports = [
    {
        "file": "plugins/org.apache.cordova.dialogs/www/notification.js",
        "id": "org.apache.cordova.dialogs.notification",
        "merges": [
            "navigator.notification"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.dialogs/www/android/notification.js",
        "id": "org.apache.cordova.dialogs.notification_android",
        "merges": [
            "navigator.notification"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.vibration/www/vibration.js",
        "id": "org.apache.cordova.vibration.notification",
        "merges": [
            "navigator.notification"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesis.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesis",
        "clobbers": [
            "window.speechSynthesis"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesisUtterance.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesisUtterance",
        "clobbers": [
            "SpeechSynthesisUtterance"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesisEvent.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesisEvent",
        "clobbers": [
            "SpeechSynthesisEvent"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesisVoice.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesisVoice",
        "clobbers": [
            "SpeechSynthesisVoice"
        ]
    },
    {
        "file": "plugins/org.apache.cordova.speech.speechsynthesis/www/SpeechSynthesisVoiceList.js",
        "id": "org.apache.cordova.speech.speechsynthesis.SpeechSynthesisVoiceList",
        "clobbers": [
            "SpeechSynthesisVoiceList"
        ]
    }
];
module.exports.metadata = 
// TOP OF METADATA
{
    "org.apache.cordova.dialogs": "0.2.9-dev",
    "org.apache.cordova.vibration": "0.3.10-dev",
    "org.apache.cordova.speech.speechsynthesis": "0.1.0"
}
// BOTTOM OF METADATA
});
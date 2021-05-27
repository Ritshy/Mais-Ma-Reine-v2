#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public enum AkCallbackType {
  AK_EndOfEvent = 0x0001,
  AK_EndOfDynamicSequenceItem = 0x0002,
  AK_Marker = 0x0004,
  AK_Duration = 0x0008,
  AK_SpeakerVolumeMatrix = 0x0010,
  AK_Starvation = 0x0020,
  AK_MusicPlaylistSelect = 0x0040,
  AK_MusicPlayStarted = 0x0080,
  AK_MusicSyncBeat = 0x0100,
  AK_MusicSyncBar = 0x0200,
  AK_MusicSyncEntry = 0x0400,
  AK_MusicSyncExit = 0x0800,
  AK_MusicSyncGrid = 0x1000,
  AK_MusicSyncUserCue = 0x2000,
  AK_MusicSyncPoint = 0x4000,
  AK_MusicSyncAll = 0x7f00,
  AK_MIDIEvent = 0x10000,
  AK_CallbackBits = 0xfffff,
  AK_EnableGetSourcePlayPosition = 0x100000,
  AK_EnableGetMusicPlayPosition = 0x200000,
  AK_EnableGetSourceStreamBuffering = 0x400000,
  AK_Monitoring = 0x20000000,
  AK_AudioSourceChange = 0x23000000,
  AK_Bank = 0x40000000,
  AK_AudioInterruption = 0x22000000
}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
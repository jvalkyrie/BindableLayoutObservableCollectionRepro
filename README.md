# BindableLayoutObservableCollectionRepro
This a sample app to demonstrate that when `Xamarin.Forms.BindableLayout.ItemSource` is an `ObservableCollection<T>`, it does not marshall `ObservableCollection.CollectionChanged` to the MainThread. As a result, it causes an error on both Android and iOS devices and consequently leads to a crash.

This behaviour has been confirmed on the latest Xamarin.Forms version (5.0.0.2012) and is identical to an issue that used to occur for the `CollectionView` (see issue 8392: https://github.com/xamarin/Xamarin.Forms/issues/8392).

The sample app contains three examples (BindableLayout, ListView and CollectionView) where a user can perform `Add()`, `RemoveAt()` and `Clear()` on each ObservableCollection<string> on a non-UI thread, to demonstrate that this issue only occurs on the `BindableLayout`:

| Scenario | `Xamarin.Forms.BindableLayout` | `Xamarin.Forms.ListView` | `Xamarin.Forms.CollectionView` |
| -------- | ------------------------------ | ------------------------ | ------------------------------ |
| `.Add()` & `ConfigureAwait(false)` | ❌ Crashes | ✅ Doesn't crash | ✅ Doesn't crash |
| `.RemoveAt()` & `ConfigureAwait(false)` | ❌ Crashes | ✅ Doesn't crash | ✅ Doesn't crash |
| `.Clear()` & `ConfigureAwait(false)` | ❌ Crashes | ✅ Doesn't crash | ✅ Doesn't crash |

## Reproduction Steps
1. Clone this repo
2. Open `BindableLayoutObservableCollectionRepro.sln` in Visual Studio
3. In Visual Studio, set `BindableLayoutObservableCollectionRepro.iOS` as the Startup Project
4. In Visual Studio, build/deploy `BindableLayoutObservableCollectionRepro.iOS` to an iOS Simulator or Device
5. Once the app has launched and the "BindableLayout" tab is selected, click on the "Add Item", "Remove Item" or "Clear List" buttons at the bottom of the screen
6. Confirm that the app crashes
7. In Visual Studio, set `BindableLayoutObservableCollectionRepro.Android` as the Startup Project
8. In Visual Studio, build/deploy `BindableLayoutObservableCollectionRepro.Android` to an Android Device or Emulator
9. Once the app has launched and the "BindableLayout" tab is selected, click on the "Add Item", "Remove Item" or "Clear List" buttons at the bottom of the screen
10. Confirm that the app crashes

### Android Crash
![Android Error](/assets/error_android.png)
```bash
[MonoDroid] UNHANDLED EXCEPTION:
[MonoDroid]   at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x00018] in /Users/builder/jenkins/workspace/archiv03-20 23:22:19.939 I/MonoDroid( 9434): Android.Util.AndroidRuntimeException: Only the original thread that created a view hierarchy can touch its views.
[MonoDroid]   at Java.Interop.JniEnvironment+InstanceMethods.CallNonvirtualVoidMethod (Java.Interop.JniObjectReference instance, Java.Interop.JniObjectReference type, Java.Interop.JniMethodInfo method, Java.Interop.JniArgumentValue* args) [0x0008e] in <42748fcc36b74733af2d9940a8f3cc8e>:0 
[MonoDroid]   at Java.Interop.JniPeerMembers+JniInstanceMethods.InvokeVirtualVoidMethod (System.String encodedMember, Java.Interop.IJavaPeerable self, Java.Interop.JniArgumentValue* parameters) [0x0005d] in <42748fcc36b74733af2d9940a8f3cc8e>:0 
[MonoDroid]   at Android.Views.ViewGroup.RemoveView (Android.Views.View view) [0x00031] in <227a96d68a0440cea172be41b1306654>:0 
[MonoDroid]   at System.Collections.ObjectModel.Collection`1[T].RemoveAt (System.Int32 index) [0x00027] in /Users/builder/jenki view) [0x0000d] in D:\a\1\s\Xamarin.Forms.Platform.Android\ViewExtensions.cs:30 
[MonoDroid]   at Xamarin.Forms.Platform.Android.VisualElementPackager.RemoveChild (Xamarin.Forms.VisualElement view) [0x00074] in D:\a\1\s\Xamarin.Forms.Platform.Android\VisualElementPackager.cs:273 
[MonoDroid]   at Xamarin.Forms.Platform.Android.VisualElementPackager.OnChildRemoved (System.Object sender, Xamarin.Forms.ElementEventArgs e) [0x00021] in D:\a\1\s\Xamarin.Forms.Platform.Android\VisualElementPackager.cs:243 
[MonoDroid]   at Xamarin.Forms.BindableLayo03-20 23:22:19.939 I/MonoDroid( 9434):   at Xamarin.Forms.Element.OnChildRemoved (Xamarin.Forms.Element child, System.Int32 oldLogicalIndex) [0x00007] in D:\a\1\s\Xamarin.Forms.Core\Element.cs:344 
[MonoDroid]   at Xamarin.Forms.VisualElement.OnChildRemoved (Xamarin.Forms.Element child, System.Int32 oldLogicalIndex) [0x00000] in D:\a\1\s\Xamarin.Forms.Core\VisualElement.cs:843 
[MonoDroid]   at Xamarin.Forms.Layout.OnInternalRemoved (Xamarin.Forms.View view, System.Int32 oldIndex) [0x00012] in D:\a\1\s\Xamarin.Forms.Core\Layout.cs:455 
[MonoDroid]   at (wrapper delegate-invoke) <Module>.invoke_void_object_NotifyCollectionChangedEventArgs(object,System.Collections.Specialized.NotifyCollectionChangedEventArgs)
[MonoDroid]   at BindableLayoutObservableCollectionRepro.ViewModels.MainPageViewModel.BindableLayoutRemoveItemAsync () [0x00025] in /Us03-20 23:22:19.940 I/MonoDroid( 9434):   at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedAction action, System.Object item, System.Int32 index) [0x00000] in /Users/builder/jenkins/workspace/archive-mono/2020-02/android/release/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:338 
[MonoDroid]   at System.Collections.ObjectModel.ObservableCollection`1[T].RemoveItem (System.Int32 index) [0x00021] in /Users/builder/jenkins/workspace/archive-mono/2020-02/android/release/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:182 
[MonoDroid]   at System.Collections.ObjectModel.Collection`1[T].RemoveAt (System.Int32 index) [0x00027] in /Users/builder/jenkins/workspace/archive-mono/2020-02/android/release/external/corefx/src/Common/src/CoreLib/System/Collections/ObjectModel/Collection.cs:144 
[MonoDroid]   at Xamarin.Forms.ObservableWrapper`2[TTrack,TRestrict].RemoveAt (System.Int32 index) [0x00035] in D:\a\1\s\Xamarin.Forms.Core\ObservableWrapper.cs:155 
[MonoDroid]   at Xamarin.Forms.BindableLayoutController+<>c__DisplayClass36_0.<ItemsSourceCollectionChanged>b__1 (System.Object item, System.Int32 index) [0x00000] in D:\a\1\s\Xamarin.Forms.Core\BindableLayout.cs:302 
[MonoDroid]   at Xamarin.Forms.Internals.NotifyCollectionChangedEventArgsExtensions.Apply (System.Collections.Specialized.NotifyCollectionChangedEventArgs self, System.Action`3[T1,T2,T3] insert, System.Action`2[T1,T2] removeAt, System.Action reset) [0x00153] in D:\a\1\s\Xamarin.Forms.Core\Internals\NotifyCollectionChangedEventArgsExtensions.cs:64 
[chatty] uid=10089(com.[redacted].bindablelayutController.ItemsSourceCollectionChanged (System.Object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x00021] in D:\a\1\s\Xamarin.Forms.Core\BindableLayout.cs:300 
[MonoDroid]   at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x00018] in /Users/builder/jenkins/workspace/archive-mono/2020-02/android/release/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:263 
[MonoDroid]   at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedAction action, System.Object item, System.Int32 index) [0x00000] in /Users/builder/jenkins/workspace/archive-mono/2020-02/android/release/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:338 
[MonoDroid]   at System.Collections.ObjectModel.Collection`1[T].RemoveAt (System.Int32 index) [0x00027] in /Users/builder/jenkins/workspace/archive-mono/2020-02/android/release/external/corefx/src/Common/src/CoreLib/System/Collections/ObjectModel/Collection.cs:144 
[MonoDroid]   at BindableLayoutObservableCollectionRepro.Services.ItemSourceService.RemoveItemAsync () [0x00093] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro/Services/ItemSourceService.cs:40 
[MonoDroid]   at BindableLayoutObservableCollectionRepro.ViewModels.MainPageViewModel.BindableLayoutRemoveItemAsync () [0x00025] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro/ViewModels/MainPageViewModel.cs:93 
[MonoDroid]   at BindableLayoutObservableCollectionRepro.ViewModels.MainPageViewModel.<get_BindableLayoutRemoveItemCommand>b__18_0 () [0x0001f] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro/ViewModels/MainPageViewModel.cs:56 
[MonoDroid]   at System.Runtime.CompilerServices.AsyncMethodBuilderCore+<>c.<ThrowAsync>b__7_0 (System.Object state) [0x00000] in /Users/builder/jenkins/workspace/archive-mono/2020-02/android/release/mcs/class/referencesource/mscorlib/system/runtime/compilerservices/AsyncMethodBuilder.cs:1021 
[MonoDroid]   at Android.App.SyncContext+<>c__DisplayClass2_0.<Post>b__0 () [0x00000] in <227a96d68a0440cea172be41b1306654>:0 
[MonoDroid]   at Java.Lang.Thread+RunnableImplementor.Run () [0x00008] in <227a96d68a0440cea172be41b1306654>:0 
[MonoDroid]   at Java.Lang.IRunnableInvoker.n_Run (System.IntPtr jnienv, System.IntPtr native__this) [0x00008] in <227a96d68a0440cea172be41b1306654>:0 
[MonoDroid]   at (wrapper dynamic-method) Android.Runtime.DynamicMethodNameCounter.1(intptr,intptr)
[MonoDroid]   --- End of managed Android.Util.AndroidRuntimeException stack trace ---
[MonoDroid] android.view.ViewRootImpl$CalledFromWrongThreadException: Only the original thread that created a view hierarchy can touch its views.
[MonoDroid] 	at android.view.ViewRootImpl.checkThread(ViewRootImpl.java:7753)
[MonoDroid] 	at android.view.ViewRootImpl.requestLayout(ViewRootImpl.java:1225)
[MonoDroid] 	at android.view.View.requestLayout(View.java:23093)
[chatty] uid=10089(com.[redacted].bindablelayoutobservablecollectionrepro) identical 4 lines
[MonoDroid] 	at android.view.View.requestLayout(View.java:23093)
[MonoDroid] 	at android.widget.RelativeLayout.requestLayout(RelativeLayout.java:360)
[MonoDroid] 	at android.view.View.requestLayout(View.java:23093)
[chatty] uid=10089(com.[redacted].bindablelayoutobservablecollectionrepro) identical 3 lines
[MonoDroid] 	at android.view.View.requestLayout(View.java:23093)
[MonoDroid] 	at androidx.recyclerview.widget.RecyclerView.requestLayout(RecyclerView.java:4412)
[MonoDroid] 	at android.view.View.requestLayout(View.java:23093)
[chatty] uid=10089(com.[redacted].bindablelayoutobservablecollectionrepro) identical 2 lines
[MonoDroid] 	at android.view.View.requestLayout(View.java:23093)
[MonoDroid] 	at android.view.ViewGroup.removeView(ViewGroup.java:5262)
```

### iOS Crash
![iOS Error](/assets/error_ios.png)
```bash
Unhandled Exception:
UIKit.UIKitThreadAccessException: UIKit Consistency error: you are calling a UIKit method that can only be invoked from the UI thread.
  at UIKit.UIApplication.EnsureUIThread () [0x0001a] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/UIKit/UIApplication.cs:95 
  at UIKit.UIView.RemoveFromSuperview () [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/UIKit/UIView.g.cs:1472 
  at Xamarin.Forms.Platform.iOS.VisualElementPackager.OnChildRemoved (Xamarin.Forms.VisualElement view) [0x00013] in D:\a\1\s\Xamarin.Forms.Platform.iOS\VisualElementPackager.cs:136 
  at Xamarin.Forms.Platform.iOS.VisualElementPackager.OnChildRemoved (System.Object sender, Xamarin.Forms.ElementEventArgs e) [0x0000f] in D:\a\1\s\Xamarin.Forms.Platform.iOS\VisualElementPackager.cs:178 
  at Xamarin.Forms.Element.OnChildRemoved (Xamarin.Forms.Element child, System.Int32 oldLogicalIndex) [0x00007] in D:\a\1\s\Xamarin.Forms.Core\Element.cs:344 
  at Xamarin.Forms.VisualElement.OnChildRemoved (Xamarin.Forms.Element child, System.Int32 oldLogicalIndex) [0x00000] in D:\a\1\s\Xamarin.Forms.Core\VisualElement.cs:843 
  at Xamarin.Forms.Layout`1[T].OnChildRemoved (Xamarin.Forms.Element child, System.Int32 oldLogicalIndex) [0x00000] in D:\a\1\s\Xamarin.Forms.Core\Layout.cs:33 
  at Xamarin.Forms.Layout.OnInternalRemoved (Xamarin.Forms.View view, System.Int32 oldIndex) [0x00012] in D:\a\1\s\Xamarin.Forms.Core\Layout.cs:455 
  at Xamarin.Forms.Layout.InternalChildrenOnCollectionChanged (System.Object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x0002b] in D:\a\1\s\Xamarin.Forms.Core\Layout.cs:418 
  at (wrapper delegate-invoke) <Module>.invoke_void_object_NotifyCollectionChangedEventArgs(object,System.Collections.Specialized.NotifyCollectionChangedEventArgs)
  at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x00018] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:263 
  at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedAction action, System.Object item, System.Int32 index) [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:338 
  at System.Collections.ObjectModel.ObservableCollection`1[T].RemoveItem (System.Int32 index) [0x00021] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:182 
  at System.Collections.ObjectModel.Collection`1[T].RemoveAt (System.Int32 index) [0x00027] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/Common/src/CoreLib/System/Collections/ObjectModel/Collection.cs:144 
  at Xamarin.Forms.ObservableWrapper`2[TTrack,TRestrict].RemoveAt (System.Int32 index) [0x00035] in D:\a\1\s\Xamarin.Forms.Core\ObservableWrapper.cs:155 
  at Xamarin.Forms.BindableLayoutController+<>c__DisplayClass36_0.<ItemsSourceCollectionChanged>b__1 (System.Object item, System.Int32 index) [0x00000] in D:\a\1\s\Xamarin.Forms.Core\BindableLayout.cs:302 
  at Xamarin.Forms.Internals.NotifyCollectionChangedEventArgsExtensions.Apply (System.Collections.Specialized.NotifyCollectionChangedEventArgs self, System.Action`3[T1,T2,T3] insert, System.Action`2[T1,T2] removeAt, System.Action reset) [0x00153] in D:\a\1\s\Xamarin.Forms.Core\Internals\NotifyCollectionChangedEventArgsExtensions.cs:64 
  at Xamarin.Forms.BindableLayoutController.ItemsSourceCollectionChanged (System.Object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x00021] in D:\a\1\s\Xamarin.Forms.Core\BindableLayout.cs:300 
  at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x00018] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:263 
  at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedAction action, System.Object item, System.Int32 index) [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:338 
  at System.Collections.ObjectModel.ObservableCollection`1[T].RemoveItem (System.Int32 index) [0x00021] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:182 
  at System.Collections.ObjectModel.Collection`1[T].RemoveAt (System.Int32 index) [0x00027] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/Common/src/CoreLib/System/Collections/ObjectModel/Collection.cs:144 
  at BindableLayoutObservableCollectionRepro.Services.ItemSourceService.RemoveItemAsync () [0x00093] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro/Services/ItemSourceService.cs:40 
  at BindableLayoutObservableCollectionRepro.ViewModels.MainPageViewModel.BindableLayoutRemoveItemAsync () [0x00025] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro/ViewModels/MainPageViewModel.cs:93 
  at BindableLayoutObservableCollectionRepro.ViewModels.MainPageViewModel.<get_BindableLayoutRemoveItemCommand>b__18_0 () [0x0001f] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro/ViewModels/MainPageViewModel.cs:56 
  at System.Runtime.CompilerServices.AsyncMethodBuilderCore+<>c.<ThrowAsync>b__7_0 (System.Object state) [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/mcs/class/referencesource/mscorlib/system/runtime/compilerservices/AsyncMethodBuilder.cs:1021 
  at Foundation.NSAsyncSynchronizationContextDispatcher.Apply () [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/Foundation/NSAction.cs:178 
--- End of stack trace from previous location where exception was thrown ---

  at (wrapper managed-to-native) UIKit.UIApplication.UIApplicationMain(int,string[],intptr,intptr)
  at UIKit.UIApplication.Main (System.String[] args, System.IntPtr principal, System.IntPtr delegate) [0x00005] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/UIKit/UIApplication.cs:86 
  at UIKit.UIApplication.Main (System.String[] args, System.String principalClassName, System.String delegateClassName) [0x0000e] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/UIKit/UIApplication.cs:65 
  at BindableLayoutObservableCollectionRepro.iOS.Application.Main (System.String[] args) [0x00001] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro.iOS/Main.cs:17
2021-03-20 23:45:36.585268+0800 BindableLayoutObservableCollectionRepro.iOS[14352:382515] Unhandled managed exception: UIKit Consistency error: you are calling a UIKit method that can only be invoked from the UI thread. (UIKit.UIKitThreadAccessException)
  at UIKit.UIApplication.EnsureUIThread () [0x0001a] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/UIKit/UIApplication.cs:95 
  at UIKit.UIView.RemoveFromSuperview () [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/UIKit/UIView.g.cs:1472 
  at Xamarin.Forms.Platform.iOS.VisualElementPackager.OnChildRemoved (Xamarin.Forms.VisualElement view) [0x00013] in D:\a\1\s\Xamarin.Forms.Platform.iOS\VisualElementPackager.cs:136 
  at Xamarin.Forms.Platform.iOS.VisualElementPackager.OnChildRemoved (System.Object sender, Xamarin.Forms.ElementEventArgs e) [0x0000f] in D:\a\1\s\Xamarin.Forms.Platform.iOS\VisualElementPackager.cs:178 
  at Xamarin.Forms.Element.OnChildRemoved (Xamarin.Forms.Element child, System.Int32 oldLogicalIndex) [0x00007] in D:\a\1\s\Xamarin.Forms.Core\Element.cs:344 
  at Xamarin.Forms.VisualElement.OnChildRemoved (Xamarin.Forms.Element child, System.Int32 oldLogicalIndex) [0x00000] in D:\a\1\s\Xamarin.Forms.Core\VisualElement.cs:843 
  at Xamarin.Forms.Layout`1[T].OnChildRemoved (Xamarin.Forms.Element child, System.Int32 oldLogicalIndex) [0x00000] in D:\a\1\s\Xamarin.Forms.Core\Layout.cs:33 
  at Xamarin.Forms.Layout.OnInternalRemoved (Xamarin.Forms.View view, System.Int32 oldIndex) [0x00012] in D:\a\1\s\Xamarin.Forms.Core\Layout.cs:455 
  at Xamarin.Forms.Layout.InternalChildrenOnCollectionChanged (System.Object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x0002b] in D:\a\1\s\Xamarin.Forms.Core\Layout.cs:418 
  at (wrapper delegate-invoke) <Module>.invoke_void_object_NotifyCollectionChangedEventArgs(object,System.Collections.Specialized.NotifyCollectionChangedEventArgs)
  at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x00018] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:263 
  at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedAction action, System.Object item, System.Int32 index) [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:338 
  at System.Collections.ObjectModel.ObservableCollection`1[T].RemoveItem (System.Int32 index) [0x00021] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:182 
  at System.Collections.ObjectModel.Collection`1[T].RemoveAt (System.Int32 index) [0x00027] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/Common/src/CoreLib/System/Collections/ObjectModel/Collection.cs:144 
  at Xamarin.Forms.ObservableWrapper`2[TTrack,TRestrict].RemoveAt (System.Int32 index) [0x00035] in D:\a\1\s\Xamarin.Forms.Core\ObservableWrapper.cs:155 
  at Xamarin.Forms.BindableLayoutController+<>c__DisplayClass36_0.<ItemsSourceCollectionChanged>b__1 (System.Object item, System.Int32 index) [0x00000] in D:\a\1\s\Xamarin.Forms.Core\BindableLayout.cs:302 
  at Xamarin.Forms.Internals.NotifyCollectionChangedEventArgsExtensions.Apply (System.Collections.Specialized.NotifyCollectionChangedEventArgs self, System.Action`3[T1,T2,T3] insert, System.Action`2[T1,T2] removeAt, System.Action reset) [0x00153] in D:\a\1\s\Xamarin.Forms.Core\Internals\NotifyCollectionChangedEventArgsExtensions.cs:64 
  at Xamarin.Forms.BindableLayoutController.ItemsSourceCollectionChanged (System.Object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x00021] in D:\a\1\s\Xamarin.Forms.Core\BindableLayout.cs:300 
  at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedEventArgs e) [0x00018] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:263 
  at System.Collections.ObjectModel.ObservableCollection`1[T].OnCollectionChanged (System.Collections.Specialized.NotifyCollectionChangedAction action, System.Object item, System.Int32 index) [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:338 
  at System.Collections.ObjectModel.ObservableCollection`1[T].RemoveItem (System.Int32 index) [0x00021] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.ObjectModel/src/System/Collections/ObjectModel/ObservableCollection.cs:182 
  at System.Collections.ObjectModel.Collection`1[T].RemoveAt (System.Int32 index) [0x00027] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/Common/src/CoreLib/System/Collections/ObjectModel/Collection.cs:144 
  at BindableLayoutObservableCollectionRepro.Services.ItemSourceService.RemoveItemAsync () [0x00093] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro/Services/ItemSourceService.cs:40 
  at BindableLayoutObservableCollectionRepro.ViewModels.MainPageViewModel.BindableLayoutRemoveItemAsync () [0x00025] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro/ViewModels/MainPageViewModel.cs:93 
  at BindableLayoutObservableCollectionRepro.ViewModels.MainPageViewModel.<get_BindableLayoutRemoveItemCommand>b__18_0 () [0x0001f] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro/ViewModels/MainPageViewModel.cs:56 
  at System.Runtime.CompilerServices.AsyncMethodBuilderCore+<>c.<ThrowAsync>b__7_0 (System.Object state) [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/mcs/class/referencesource/mscorlib/system/runtime/compilerservices/AsyncMethodBuilder.cs:1021 
  at Foundation.NSAsyncSynchronizationContextDispatcher.Apply () [0x00000] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/Foundation/NSAction.cs:178 
--- End of stack trace from previous location where exception was thrown ---

  at (wrapper managed-to-native) UIKit.UIApplication.UIApplicationMain(int,string[],intptr,intptr)
  at UIKit.UIApplication.Main (System.String[] args, System.IntPtr principal, System.IntPtr delegate) [0x00005] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/UIKit/UIApplication.cs:86 
  at UIKit.UIApplication.Main (System.String[] args, System.String principalClassName, System.String delegateClassName) [0x0000e] in /Library/Frameworks/Xamarin.iOS.framework/Versions/14.0.0.0/src/Xamarin.iOS/UIKit/UIApplication.cs:65 
  at BindableLayoutObservableCollectionRepro.iOS.Application.Main (System.String[] args) [0x00001] in /Users/[redacted]/Projects/BindableLayoutObservableCollectionRepro/BindableLayoutObservableCollectionRepro.iOS/Main.cs:17
```

### Environment

```bash
Visual Studio Community 2019 for Mac
Version 8.7.8 (build 4)
Installation UUID: 80666063-120c-4b6a-a58a-3e2aecd76fb9
	GTK+ 2.24.23 (Raleigh theme)
	Xamarin.Mac 6.18.0.23 (d16-6 / 088c73638)

	Package version: 612000093

Mono Framework MDK
Runtime:
	Mono 6.12.0.93 (2020-02/620cf538206) (64-bit)
	Package version: 612000093

Roslyn (Language Service)
3.7.0-6.20427.1+18ede13943b0bfae1b44ef078b2f3923159bcd32

NuGet
Version: 5.7.0.6702

.NET Core SDK
SDK: /usr/local/share/dotnet/sdk/3.1.402/Sdks
SDK Versions:
	3.1.402
	3.1.200
	3.0.101
	3.0.100
MSBuild SDKs: /Library/Frameworks/Mono.framework/Versions/6.12.0/lib/mono/msbuild/Current/bin/Sdks

.NET Core Runtime
Runtime: /usr/local/share/dotnet/dotnet
Runtime Versions:
	3.1.8
	3.1.2
	3.0.1
	3.0.0
	2.1.22
	2.1.16
	2.1.14
	2.1.13

Xamarin.Profiler
Version: 1.6.12.29
Location: /Applications/Xamarin Profiler.app/Contents/MacOS/Xamarin Profiler

Updater
Version: 11

Apple Developer Tools
Xcode 12.3 (17715)
Build 12C33

Xamarin Designer
Version: 16.7.0.495
Hash: 03d50a221
Branch: remotes/origin/d16-7-vsmac
Build date: 2020-08-28 13:12:52 UTC

Xamarin.Mac
Version: 6.20.2.2 (Visual Studio Community)
Hash: 817b6f72a
Branch: d16-7
Build date: 2020-07-18 18:44:59-0400

Xamarin.iOS
Version: 14.0.0.0 (Visual Studio Community)
Hash: 7ec3751a1
Branch: xcode12
Build date: 2020-09-16 11:33:15-0400

Xamarin.Android
Version: 11.0.2.0 (Visual Studio Community)
Commit: xamarin-android/d16-7/025fde9
Android SDK: /Users/[redacted]/Library/Developer/Xamarin/android-sdk-macosx
	Supported Android versions:
		None installed

SDK Tools Version: 26.1.1
SDK Platform Tools Version: 29.0.6
SDK Build Tools Version: 29.0.2

Build Information: 
Mono: 83105ba
Java.Interop: xamarin/java.interop/d16-7@1f3388a
ProGuard: Guardsquare/proguard/proguard6.2.2@ebe9000
SQLite: xamarin/sqlite/3.32.1@1a3276b
Xamarin.Android Tools: xamarin/xamarin-android-tools/d16-7@017078f

Microsoft OpenJDK for Mobile
Java SDK: /Users/[redacted]/Library/Developer/Xamarin/jdk/microsoft_dist_openjdk_1.8.0.25
1.8.0-25
Android Designer EPL code available here:
https://github.com/xamarin/AndroidDesigner.EPL

Android SDK Manager
Version: 16.7.0.13
Hash: 8380518
Branch: remotes/origin/d16-7~2
Build date: 2020-09-16 05:12:24 UTC

Android Device Manager
Version: 16.7.0.24
Hash: bb090a3
Branch: remotes/origin/d16-7
Build date: 2020-09-16 05:12:46 UTC

Build Information
Release ID: 807080004
Git revision: 9ea7bef96d65cdc3f4288014a799026ccb1993bc
Build date: 2020-09-16 17:22:54-04
Build branch: release-8.7
Xamarin extensions: 9ea7bef96d65cdc3f4288014a799026ccb1993bc

Operating System
Mac OS X 10.15.7
Darwin 19.6.0 Darwin Kernel Version 19.6.0
    Mon Aug 31 22:12:52 PDT 2020
    root:xnu-6153.141.2~1/RELEASE_X86_64 x86_64
```

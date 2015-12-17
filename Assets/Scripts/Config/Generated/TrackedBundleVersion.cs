using System.Collections;

public class TrackedBundleVersion
{
	public static readonly string bundleIdentifier = "com.diegochiodini.ballyball";

	public static readonly TrackedBundleVersionInfo Version_0_1_2 =  new TrackedBundleVersionInfo ("0.1.2", 0);
	
	public ArrayList history = new ArrayList ();

	public TrackedBundleVersionInfo current = new TrackedBundleVersionInfo ("0.1.2", 0);

	public  TrackedBundleVersion() {
		history.Add (current);
	}

}

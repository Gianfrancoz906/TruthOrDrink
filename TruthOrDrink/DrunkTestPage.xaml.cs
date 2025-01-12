namespace TruthOrDrink;

public partial class DrunkTestPage : ContentPage
{
    private double tiltX;
    private double tiltY;
    private double tiltZ;
    private System.Timers.Timer timer;

    public DrunkTestPage()
    {
        InitializeComponent();

        timer = new System.Timers.Timer(500);
        timer.Elapsed += Timer_Elapsed;
        timer.AutoReset = true;


        Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
        Gyroscope.Start(SensorSpeed.UI);
    }

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // Update the label with the latest tilt information
        Device.BeginInvokeOnMainThread(() =>
        {
            lblTiltX.Text = $" {(int)Math.Abs(tiltX * 100)}";
            lblTiltY.Text = $" {(int)Math.Abs(tiltY * 100)}";
            lblTiltZ.Text = $" {(int)Math.Abs(tiltZ * 100)}";




        });
    }

    private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
    {
        // Update tilt values
        tiltX = e.Reading.AngularVelocity.X;
        tiltY = e.Reading.AngularVelocity.Y;
        tiltZ = e.Reading.AngularVelocity.Z;

        //Start the timer if not already running
        if (!timer.Enabled)
            timer.Start();
    }


}
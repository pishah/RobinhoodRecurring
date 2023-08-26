using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace RobinhoodRecurring;

public partial class MainPage : ContentPage
{
	int count = 0;
    public double rH = 0;
    public MainPage()
    {
        InitializeComponent();
        this.Loaded += MainPage_Loaded;
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
        //animate in the default content
        //set the default grid displayed to 0 scale so we can animate it back in
        await gridText.ScaleTo(0, 0, null);
        //call the method to animate it in
        await setScaleAnimation(1000, 1, Easing.CubicInOut, 0, 0, null);
    }
    //method to animate in the two different grid contents in and out
    private async Task setScaleAnimation(uint defaultScaleTime, double DefaultScaleValue, Easing easeDefault, uint ValueScaleTime, double ValueScaleValue, Easing easeValue) {
        //set the Grid with the Value Content animation scale
        await Task.Run(() => {
            gridValue.ScaleTo(ValueScaleValue, ValueScaleTime, easeValue);
            gridText.ScaleTo(DefaultScaleValue, defaultScaleTime, easeDefault);
        });
    }

    //gesture events for our link to Robinhood to get the data
    //click event to open the Robinhood URL to get the text we need
    private async void ClickGestureRecognizer_ClickedRobin(object sender, EventArgs e)
    {
        Uri uri = new Uri("https://robinhood.com/account/recurring");
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }
    //gesture pointer event to change text so user knows they are over it
    private void PointerGestureRecognizer_PointerEnteredRobin(object sender, PointerEventArgs e)
    {
        txtHelper.TextColor = Colors.Black;
        txtHelper.FontAttributes = FontAttributes.Italic;
    }
    //gesture event to change the text back so user knows they are no longer over it
    private void PointerGestureRecognizer_PointerExitedRobin(object sender, PointerEventArgs e)
    {

        txtHelper.TextColor = Colors.White;
        txtHelper.FontAttributes = FontAttributes.None;
    }
    private async void btnPasted_Clicked(object sender, EventArgs e)
    {
        //check to see if there is any data in the TextBox if not return and stop the method
        if (txtText.Text == null) {
            //Maui default popup alert/messagebox
            await DisplayAlert("Alert", "Please Enter Text", "OK");
            return;
        }
        if (txtText.Text.Length <= 0) {
            //Maui default popup alert/messagebox
            await DisplayAlert("Alert", "Please Enter Text", "OK");
            return;
        }
        //setup some Lists we can hold and manilate data with
        //this will hold each line of the Editor Entry
        List<string> list = new List<string>();
        //list to hold the the value of the Buy we can sum up
        List<double> listDouble = new List<double>();
        //here we use regex to split the txt into seperate lines we can then loop through and add to our List
        var tt = Regex.Split(txtText.Text, "\r\n|\r|\n").ToList();
        foreach (var l in tt)
        {
            list.Add(l);
        }
        //here we set a start value "s" and then loop through each line in our "list" of strings we just created from the lines. The reason we do this and not foreach like above is I thought it is a bit easier to understand the index "s" value to reference for finding the line with the value, you can do it different ways
        int s = 0;
        while (s < list.Count)
        {
            //check to see if the line is a Daily Buy
            if (list[s].Contains("Daily Buy"))
            {
                //in the data, the line with the value is the second line below the Daily Buy so we use the Index + 2 to get that line. Would be more future proof for issues to loop through and find next line with $ value, I might change this in the future.
                var ll2 = list[(int)s + 2];
                //remove the $ text value from the string to just have the number
                ll2 = ll2.Replace("$", "");
                //convert the new value to a type of Double and add it to our List of doubles to Sum up later
                listDouble.Add(Convert.ToDouble(ll2));
            }
            //increase our index for the loop. We could have also done s++ in the loop setup itself but I feel it is easier to read it by increasing it at the end.   
            s++;
        }
        //set our Label with the Sum of our Values. Linq has a built in Sum method to make this easy
        lblTotal.Text = listDouble.Sum(x => x).ToString();
        //call our Scale Method with the values to show the Value content and hide the default content
        await setScaleAnimation(1000, 0, Easing.CubicInOut, 1000, 1, Easing.BounceIn);
    }
    //Button click event for resetting the view after seeing the Value
    private async void btnReset_Clicked(object sender, EventArgs e)
    {
        //set the Grid sizes back to default so we can enter in new text
        await setScaleAnimation(1000, 1, Easing.CubicInOut, 0, 0, null);
    }
    //footer gesture methods
    //company click event to open URL to the Website
    private async void ClickGestureRecognizer_Clicked(object sender, EventArgs e)
    {

        Uri uri = new Uri("https://pishah.com");
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }
    //gesture pointer event to change text so user knows they are over it
    private void PointerGestureRecognizer_PointerEntered(object sender, PointerEventArgs e)
    {
        txtTrademark.TextColor = Colors.Black;
        txtTrademark.FontAttributes = FontAttributes.Italic;
    }
    //gesture event to change the text back so user knows they are no longer over it
    private void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
    {

        txtTrademark.TextColor = Colors.White;
        txtTrademark.FontAttributes = FontAttributes.None;
    }
    
}



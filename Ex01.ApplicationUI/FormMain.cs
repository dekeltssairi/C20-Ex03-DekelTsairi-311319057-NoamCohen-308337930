using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FacadeLayer;
using FacebookWrapper.ObjectModel;
using MRG.Controls.UI;

namespace Ex01.ApplicationUI
{
    public partial class FormMain : Form
    {
        public UISetting UISettings { get; set; }
        public FormDesigner FormDesigner{ get; set; }

        public FormMain()
        {
            InitializeComponent();
            f_ButtonLogout.Click += this.buttonLogout_Click;
            f_ButtonLogout.Click += this.clearForm;
            if (FacadeLayer.FacadeFacebook.Instance.ShouldRestoreSettings())
            {
                restoreSettings();
                FacadeLayer.FacadeFacebook.Instance.Connect();
                userBindingSource.DataSource = FacadeLayer.FacadeFacebook.Instance.LoggedUser;
                handleControlsVisibility();
            }
        }

        private void handleControlsVisibility()
        {
            handleLabels();
            handleButtons();
            f_CheckBoxRememberMe.Visible = !f_CheckBoxRememberMe.Visible;
        }

        private void handleLabels()
        {
            foreach (Label label in f_PanelProfile.Controls.OfType<Label>())
            {
                label.Visible = !label.Visible;
            }
        }

        private void handleButtons()
        {
            foreach (Button button in splitContainer1.Panel2.Controls.OfType<Button>())
            {
                button.Enabled = !button.Enabled;
            }

            f_ButtonLogin.Enabled = false;
        }

        private void restoreSettings()
        {
            UISettings = new UISetting().LoadFromFile() as UISetting;
            applySettings();
        }

        private void applySettings()
        {
            StartPosition = FormStartPosition.Manual;
            f_CheckBoxRememberMe.Checked = UISettings.RememberMeCheckBox;
            Location = UISettings.LastWindowLocation;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            FacadeLayer.FacadeFacebook.Instance.LogIn();
            userBindingSource.DataSource = FacadeLayer.FacadeFacebook.Instance.LoggedUser;
            handleControlsVisibility();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            FacadeLayer.FacadeFacebook.Instance.Logout();
        }

        private void clearForm(object sender, EventArgs e)
        {
            this.Dispose(false);
            new FormMain().Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (f_CheckBoxRememberMe.Checked && FacadeLayer.FacadeFacebook.Instance.IsUserLogin())
            {
                FacadeLayer.FacadeFacebook.Instance.LogicSettings.RememberUser = f_CheckBoxRememberMe.Checked;
                FacadeLayer.FacadeFacebook.Instance.LogicSettings.LastAccessToken = FacadeLayer.FacadeFacebook.Instance.LoginResult.AccessToken;
                FacadeLayer.FacadeFacebook.Instance.LogicSettings.SaveToFile();
                saveUiSettings();
            }
            else
            {
                FacadeLayer.FacadeFacebook.Instance.LogicSettings.LastAccessToken = null;
            }

            FacadeLayer.FacadeFacebook.Instance.LogicSettings.SaveToFile();
        }

        private void saveUiSettings()
        {
            UISettings = new UISetting
            {
                LastWindowSize = Size,
                LastWindowLocation = Location,
                RememberMeCheckBox = f_CheckBoxRememberMe.Checked
            };

            UISettings.SaveToFile();
        }

        private void onFetchPostsThread()
        {
            foreach (Post post in FacadeLayer.FacadeFacebook.Instance.GetLoggedUserPosts())
            {
                f_ListBoxPosts.Invoke(new Action(() => f_ListBoxPosts.Items.Add(new PostProxy { Post = post })));
            }

            if (!FacadeLayer.FacadeFacebook.Instance.IsLoggedUserHasPosts())
            {
                MessageBox.Show("No Posts to retrieve :(");
            }
        }

        private void onShowFriendsThread()
        {
            if (!FacadeLayer.FacadeFacebook.Instance.isLoggedUserHasFriends())
            {
                MessageBox.Show("No Friends to retrieve :(");
            }
            else
            {
                new FormFriendList(FacadeLayer.FacadeFacebook.Instance.GetLoggedUserFriends(), f_LoadingCircleShowFriend).ShowDialog();
            }
        }

        private void onShowCheckinsThread()
        {
            if (!FacadeLayer.FacadeFacebook.Instance.IsLoggedUserHasCheckins())
            {
                MessageBox.Show("No Posts to retrieve :(");
            }
            else
            {
                new FormCheckinList(FacadeLayer.FacadeFacebook.Instance.GetCheckins(), f_LoadingCircleShowCheckins).ShowDialog();
            }
        }

        private void onFetchEventsThread()
        {
            if (!FacadeLayer.FacadeFacebook.Instance.IsLoggedUserHasEvents())
            {
                MessageBox.Show("No Events to retrieve :(");
            }
            else
            {
                new FormEventsList(FacadeLayer.FacadeFacebook.Instance.GetLoggedUserEvents(), f_LoadingCircleShowEvents).ShowDialog();
            }
        }

        private void onMostDiggingFriendThread()
        {
            if (!FacadeLayer.FacadeFacebook.Instance.isLoggedUserHasFriends())
            {
                MessageBox.Show("You Don't have any friends :(");
            }
            else
            {
                DigginFriend digginFriend = FacadeLayer.FacadeFacebook.Instance.GetDigginFriend();

                new FormMosiftDiggingFriend(digginFriend, f_LoadingCircleShowMostDiggingFriend).ShowDialog();
            }
        }

        private void Covid19_button_Click(object sender, EventArgs e)
        {
            try
            {
                new FormCovid19CheckedIn(FacadeLayer.FacadeFacebook.Instance.LoggedUser).ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("You dont have any chekins");
            }
        }

        private void onShowAlbumsThread()
        {
            if (!FacadeLayer.FacadeFacebook.Instance.IsLoggedUserHasAlbums())
            {
                MessageBox.Show("No Albums to retrieve :(");
            }
            else
            {
                new FormAlbums(FacadeLayer.FacadeFacebook.Instance.GetLoggedUserAlbums(), f_LoadingCircleShowMyAlbums).ShowDialog();
            }
        }

        private void activeLoadingCircle(LoadingCircle i_LoadingCircle)
        {
            i_LoadingCircle.Visible = true;
            i_LoadingCircle.Active = true;
        }

        private void f_ButtonShowLikes_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Out of Permission!");
        }

        private void f_Postbutton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Out of Permission!");
        }

        private void f_ButtonShowMyPosts_Click(object sender, EventArgs e)
        {
            f_ListBoxPosts.Items.Clear();
            new Thread(onFetchPostsThread).Start();
        }

        private void f_ButtonShowMyEvents_Click(object sender, EventArgs e)
        {
            activeLoadingCircle(f_LoadingCircleShowEvents);
            new Thread(onFetchEventsThread).Start();
        }

        private void f_ButtonMyAlbums_Click(object sender, EventArgs e)
        {
            activeLoadingCircle(f_LoadingCircleShowMyAlbums);
            new Thread(onShowAlbumsThread).Start();
        }

        private void buttonMostDiggingFriend_Click(object sender, EventArgs e)
        {
            activeLoadingCircle(f_LoadingCircleShowMostDiggingFriend);
            new Thread(onMostDiggingFriendThread).Start();
        }

        private void f_CheckinsButton_Click(object sender, EventArgs e)
        {
            activeLoadingCircle(f_LoadingCircleShowCheckins);
            new Thread(onShowCheckinsThread).Start();
        }

        private void f_ShowFriendsButton_Click(object sender, EventArgs e)
        {
            activeLoadingCircle(f_LoadingCircleShowFriend);
            new Thread(onShowFriendsThread).Start();
        }

        private void f_ButtonUIHeight_Click(object sender, EventArgs e)
        {
            FormDesigner = new HeightFormDesigner() { Form = this };
            FormDesigner.design();
        }

        private void f_ButtonUIMix_Click(object sender, EventArgs e)
        {
            acceptVisitor(new MixFormDesigner() { Form = this });
        }

        private void f_ButtonUIYellow_Click(object sender, EventArgs e)
        {
            acceptVisitor(new YellowFormDesigner() { Form = this });
        }

        private void f_ButtonFedaultUiHeight_Click(object sender, EventArgs e)
        {
            acceptVisitor(new DefaultFormDesigner() { Form = this });
        }

        private void acceptVisitor(FormDesigner i_FormDesigner)
        {
            FormDesigner = i_FormDesigner;
            FormDesigner.design();
        }
    }
}

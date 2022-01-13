using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Binding;
using ReactiveUI;
using System.Application.UI.Activities;
using System.Application.UI.Adapters;
using System.Application.UI.Resx;
using System.Application.UI.ViewModels;
using static System.Application.UI.ViewModels.LoginOrRegisterWindowViewModel;

namespace System.Application.UI.Fragments
{
    [Register(JavaPackageConstants.Fragments + nameof(FastLoginOrRegisterFragment))]
    internal sealed class FastLoginOrRegisterFragment : BaseFragment<fragment_login_and_register_by_fast, LoginOrRegisterWindowViewModel>
    {
        protected override int? LayoutResource => Resource.Layout.fragment_login_and_register_by_fast;

        public override void OnCreateView(View view)
        {
            base.OnCreateView(view);

            binding!.tvAgreementAndPrivacy.SetLinkMovementMethod();

            R.Subscribe(() =>
            {
                if (binding == null) return;
                binding.tvTip.Text = AppResources.User_FastLoginTip;
                binding.tvAgreementAndPrivacy.TextFormatted = LoginOrRegisterActivity.CreateAgreementAndPrivacy(ViewModel!);
            }).AddTo(this);

            var adapter = new FastLoginChannelAdapter(ViewModel!);
            adapter.ItemClick += (_, e) =>
            {
                switch (e.Current.Id)
                {
                    case FastLoginChannelViewModel.PhoneNumber:
                        GoToUsePhoneNumberPage(e.GetView());
                        break;
                    default:
                        ViewModel!.ChooseChannel.Invoke(e.Current.Id);
                        break;
                }
            };
            binding.rvFastLoginChannels.SetLinearLayoutManager();
            binding.rvFastLoginChannels.AddVerticalItemDecorationIdRes(Resource.Dimension.fast_login_or_register_margin_subtract_compat_padding);
            binding.rvFastLoginChannels.SetAdapter(adapter);

            if (Activity is AppCompatActivity activity)
            {
                activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }
        }

        void GoToUsePhoneNumberPage(View? view)
        {
            var navController = this.GetNavController(view);
            navController?.Navigate(Resource.Id.action_navigation_login_or_register_fast_to_navigation_login_or_register_phone_number);
        }
    }
}
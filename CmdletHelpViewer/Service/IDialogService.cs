﻿using System.Windows;


namespace CmdletHelpViewer.Service
{
	public interface IDialogService
	{
		/// <summary>
		/// Registers a View.
		/// </summary>
		/// <param name="view">The registered View.</param>
		void Register(FrameworkElement view);


		/// <summary>
		/// Unregisters a View.
		/// </summary>
		/// <param name="view">The unregistered View.</param>
		void Unregister(FrameworkElement view);

       
		/// <summary>
		/// Shows a dialog.
		/// </summary>
		/// <param name="ownerViewModel">A ViewModel that represents the owner window of the
		/// dialog.</param>
		/// <param name="viewModel">The ViewModel of the new dialog.</param>
		/// <returns>A nullable value of type bool that signifies how a window was closed by the
		/// user.</returns>
		bool? ShowDialog<T>(object ownerViewModel, object viewModel) where T : Window;


		/// <summary>
		/// Shows a message box.
		/// </summary>
		/// <param name="ownerViewModel">A ViewModel that represents the owner window of the message
		/// box.</param>
		/// <param name="messageBoxText">A string that specifies the text to display.</param>
		/// <param name="caption">A string that specifies the title bar caption to display.</param>
		/// <param name="button">A MessageBoxButton value that specifies which button or buttons to
		/// display.</param>
		/// <param name="icon">A MessageBoxImage value that specifies the icon to display.</param>
		/// <returns>A MessageBoxResult value that specifies which message box button is clicked by the
		/// user.</returns>
		MessageBoxResult ShowMessageBox(object ownerViewModel, string messageBoxText, string caption,
			MessageBoxButton button, MessageBoxImage icon);



	}
}

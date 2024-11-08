﻿using BlazingChat.Shared.Dtos;
using System.ComponentModel;

namespace BlazingChat.Client.States
{
    public class AuthenticationState : INotifyPropertyChanged
    {
        public const string AuthStoreKey = "authKey";
        
        public event PropertyChangedEventHandler? PropertyChanged;

        public UserDto User { get; set; } = default;
        public string? Token { get; set; }

        private bool _isAuthenticated;

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAuthenticated)));
                }
            }
        }

        public void LoadState(AuthResponseDto authResponseDto)
        {
            User = authResponseDto.User;
            Token = authResponseDto.Token;
            IsAuthenticated = true;
        }

        public void UnLoadState()
        {
            User = default;
            Token = null;
            IsAuthenticated = false;
        }
    }
}

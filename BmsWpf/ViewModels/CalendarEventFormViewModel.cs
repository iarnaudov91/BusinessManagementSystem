﻿namespace BmsWpf.ViewModels
{
    using BMS.DataBaseModels.Enums;
    using BmsWpf.Behaviour;
    using BmsWpf.Services.Contracts;
    using BmsWpf.Services.DTOs;
    using BmsWpf.Views.ChildWindows;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class CalendarEventFormViewModel : ViewModelBase, IPageViewModel
    {
        private ProjectListDto selectedProject;
        private ObservableCollection<ProjectListDto> projectsList;
        private int id;
        private int creatorId;
        private string eventCreator;
        private string title;
        private string description;
        private DateTime startDate;
        private DateTime endDate;
        private Color selectedColor;
        private readonly ObservableCollection<Color> colors;


        public ICommand WindowLoadedCommand;
        public ICommand SaveCommand;
        public ICommand BackCommand;
        public DataRowView selectedCalendarEvent;

        public CalendarEventFormViewModel()
        {
            this.startDate = DateTime.Now;
            this.endDate = DateTime.Now;

            var colorsList = Enum.GetValues(typeof(Color)).Cast<Color>().ToList();
            this.colors = new ObservableCollection<Color>(colorsList);
        }

        public ICalendarEventService CalendarEventService { get; set; }
        public IUserService UserService { get; set; }
        public IProjectService ProjectService { get; set; }
        public IViewManager ViewManager { get; set; }

        public Action CloseAction { get; set; }

        public string ViewName
        {
            get
            {
                return "Calendar Event Form";
            }
        }

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                this.OnPropertyChanged(nameof(Id));
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                this.OnPropertyChanged(nameof(this.Title));
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
                this.OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
                this.OnPropertyChanged(nameof(this.StartDate));
            }
        }
        public DateTime EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = value;
                this.OnPropertyChanged(nameof(this.EndDate));
            }
        }

        public DataRowView SelectedCalendarEvent
        {
            get
            {
                return this.selectedCalendarEvent;
            }
            set
            {
                this.selectedCalendarEvent = value;
                this.OnPropertyChanged(nameof(this.SelectedCalendarEvent));
            }
        }

        public string EventCreator
        {
            get
            {
                return this.eventCreator;
            }
            set
            {
                this.eventCreator = value;
                this.OnPropertyChanged(nameof(this.EventCreator));
            }
        }

        public ProjectListDto SelectedProject
        {
            get
            {
                return this.selectedProject;
            }
            set
            {
                this.selectedProject = value;
                this.OnPropertyChanged(nameof(this.SelectedProject));
            }
        }

        public ObservableCollection<ProjectListDto> ProjectsList
        {
            get
            {
                return this.projectsList;
            }
            set
            {
                this.projectsList = value;
                OnPropertyChanged(nameof(this.ProjectsList));
            }
        }

        public Color SelectedColor
        {
            get
            {
                return this.selectedColor;
            }
            set
            {
                this.selectedColor = value;
                this.OnPropertyChanged(nameof(this.SelectedColor));
            }
        }

        public ObservableCollection<Color> Colors
        {
            get
            {
                return this.colors;
            }
        }

        public ICommand WindowLoaded
        {
            get
            {
                if (this.WindowLoadedCommand == null)
                {
                    this.WindowLoadedCommand = new RelayCommand(this.HandleLoadedCommand);
                }
                return this.WindowLoadedCommand;
            }
        }


        public ICommand Save
        {
            get
            {
                if (this.SaveCommand == null)
                {
                    this.SaveCommand = new RelayCommand(this.HandleSaveCommand);
                }
                return this.SaveCommand;
            }
        }

        public ICommand Back
        {
            get
            {
                if (this.BackCommand == null)
                {
                    this.BackCommand = new RelayCommand(this.HandleBackCommand);
                }
                return this.BackCommand;
            }
        }

        private void HandleLoadedCommand(object parameter)
        {
            this.ProjectsList = new ObservableCollection<ProjectListDto>(this.ProjectService.GetProjectsForDropdown());
            this.creatorId = this.UserService.GetUsernames().SingleOrDefault(x => x.Username == EventCreator).Id;
            if (this.SelectedCalendarEvent != null)
            {
                this.Id = (int)this.selectedCalendarEvent.Row.ItemArray[0];
                this.Title = (string)this.selectedCalendarEvent.Row.ItemArray[1];
                this.Description = (string)this.selectedCalendarEvent.Row.ItemArray[2];
                this.StartDate = (DateTime)this.selectedCalendarEvent.Row.ItemArray[3];
                this.EndDate = (DateTime)this.selectedCalendarEvent.Row.ItemArray[4];
                this.selectedColor = (Color)this.selectedCalendarEvent.Row.ItemArray[5];
                var projectDto = (ProjectListDto)SelectedCalendarEvent.Row.ItemArray[7];//You saw this one! It is important to do this selection, because it will not select username if you directly pass the dto.
                this.SelectedProject = ProjectsList.SingleOrDefault(x => x.Id == projectDto.Id);
            }
        }

        private void HandleSaveCommand(object parameter)
        {
            var result = string.Empty;
            try
            {
                var newCalendarEvent = new CalendarEventsPostDto()
                {
                    Id = this.Id,
                    CreatorId = this.creatorId,
                    ProjectId = this.SelectedProject.Id,
                    Title = this.Title,
                    Description = this.Description,
                    StartDate = this.StartDate,
                    EndDate = this.EndDate,
                    Color = this.SelectedColor,
                };

                if (this.SelectedCalendarEvent == null)
                {
                    result = this.CalendarEventService.CreateCalendarEvent(newCalendarEvent);
                    MessageBox.Show(result);
                    this.RedirectToMainCalendarEvents();
                }
                else
                {
                    result = this.CalendarEventService.EditCalendarEvent(newCalendarEvent);
                    MessageBox.Show(result);
                    this.RedirectToMainCalendarEvents();
                }
            }
            catch (Exception e)
            {
                result = e.Message;
                MessageBox.Show(result);
            }

        }

        private void HandleBackCommand(object parameter)
        {
            this.RedirectToMainCalendarEvents();
        }

        private void RedirectToMainCalendarEvents()
        {
            var mainCalendarEventWindow = this.ViewManager.ComposeObjects<MainCalendarEvents>();
            mainCalendarEventWindow.Show();
            this.CloseAction();
        }

    }
}

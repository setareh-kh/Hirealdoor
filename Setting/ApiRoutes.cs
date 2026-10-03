namespace Hirealdoor.Setting;

public class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;
    private const string AdminBase = Root + "/" + Version + "/admin";
    private const string ProfileBase = Root + "/" + Version + "/profile";

    // /api/v1/admin/*
    public static class Admin
    {
        // /api/v1/admin/persons
        public const string Person = AdminBase + "/persons";
        public const string User = AdminBase + "/users";
    }

    // /api/v1/profile/*
    public static class Profile
    {
        // /api/v1/profile/persons
        public const string Person = ProfileBase + "/persons";
        public const string User = ProfileBase + "/users";
    }

    public static class Website
    {
        // /api/v1/persons
        public const string Person = Base + "/persons";
        public const string User = Base + "/users";
    }
}
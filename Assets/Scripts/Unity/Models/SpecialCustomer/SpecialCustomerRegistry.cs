using System.Collections.Generic;
using System.Linq;

public class SpecialCustomerRegistry
{
    public const string DEAN_IDENTIFIER = "dean";
    public static SpecialCustomerModel DEAN = new SpecialCustomerModel(DEAN_IDENTIFIER, new SpecialCustomerSpriteDescriptor("dean_front.png", "dean_back.png"));

    public const string TEAMPLAY_MVP_IDENTIFIER = "teamplay_mvp";
    public static SpecialCustomerModel TEAMPLAY_MVP = new SpecialCustomerModel(TEAMPLAY_MVP_IDENTIFIER, new SpecialCustomerSpriteDescriptor("teamplay_mvp_front.png", "teamplay_mvp_back.png"));
    
    public const string BROKEN_GIRL_IDENTIFIER = "broken_girl";
    public static SpecialCustomerModel BROKEN_GIRL = new SpecialCustomerModel(BROKEN_GIRL_IDENTIFIER, new SpecialCustomerSpriteDescriptor("broken_girl_front.png", "broken_girl_back.png"));
    
    public const string COUPLE_IDENTIFIER = "couple";
    public static SpecialCustomerModel COUPLE = new SpecialCustomerModel(COUPLE_IDENTIFIER, new SpecialCustomerSpriteDescriptor("couple_front.png", "couple_back.png"));

    public const string SOLO_EATER_IDENTIFIER = "solo_eater";
    public static SpecialCustomerModel SOLO_EATER = new SpecialCustomerModel(SOLO_EATER_IDENTIFIER, new SpecialCustomerSpriteDescriptor("solo_eater_front.png", "solo_eater_back.png"));

    public const string FRESHMAN_IDENTIFIER = "freshman";
    public static SpecialCustomerModel FRESHMAN = new SpecialCustomerModel(FRESHMAN_IDENTIFIER, new SpecialCustomerSpriteDescriptor("freshman_front.png", "freshman_back.png"));

    public const string RAIN_WETTED_IDENTIFIER = "rain_wetted";
    public static SpecialCustomerModel RAIN_WETTED = new SpecialCustomerModel(RAIN_WETTED_IDENTIFIER, new SpecialCustomerSpriteDescriptor("rain_wetted_front.png", "rain_wetted_back.png"));

    public const string F_GIVER_PROF_IDENTIFIER = "f_giver_prof";
    public static SpecialCustomerModel F_GIVER_PROF = new SpecialCustomerModel(F_GIVER_PROF_IDENTIFIER, new SpecialCustomerSpriteDescriptor("f_giver_prof_front.png", "f_giver_prof_back.png"));

    public const string TEST_PREPARER_IDENTIFIER = "test_preparer";
    public static SpecialCustomerModel TEST_PREPARER = new SpecialCustomerModel(TEST_PREPARER_IDENTIFIER, new SpecialCustomerSpriteDescriptor("test_preparer_front.png", "test_preparer_back.png"));

    public const string EARLY_BIRD_IDENTIFIER = "early_bird";
    public static SpecialCustomerModel EARLY_BIRD = new SpecialCustomerModel(EARLY_BIRD_IDENTIFIER, new SpecialCustomerSpriteDescriptor("early_bird_front.png", "early_bird_back.png"));

    public const string APPLY_FAILER_IDENTIFIER = "apply_failer";
    public static SpecialCustomerModel APPLY_FAILER = new SpecialCustomerModel(APPLY_FAILER_IDENTIFIER, new SpecialCustomerSpriteDescriptor("apply_failer_front.png", "apply_failer_back.png"));
    
    public const string SENIOR_AND_JUNIOR_IDENTIFIER = "senior_and_junior";
    public static SpecialCustomerModel SENIOR_AND_JUNIOR = new SpecialCustomerModel(SENIOR_AND_JUNIOR_IDENTIFIER, new SpecialCustomerSpriteDescriptor("senior_and_junior_front.png", "senior_and_junior_back.png"));

    public const string GRASS_ENJOYER_IDENTIFIER = "grass_enjoyer";
    public static SpecialCustomerModel GRASS_ENJOYER = new SpecialCustomerModel(GRASS_ENJOYER_IDENTIFIER, new SpecialCustomerSpriteDescriptor("grass_enjoyer_front.png", "grass_enjoyer_back.png"));
    
    public const string TOP_STUDENT_IDENTIFIER = "top_student";
    public static SpecialCustomerModel TOP_STUDENT = new SpecialCustomerModel(TOP_STUDENT_IDENTIFIER, new SpecialCustomerSpriteDescriptor("top_student_front.png", "top_student_back.png"));

    public const string DEVELOPER_IDENTIFIER = "developer";
    public static SpecialCustomerModel DEVELOPER = new SpecialCustomerModel(DEVELOPER_IDENTIFIER, new SpecialCustomerSpriteDescriptor("developer_front.png", "developer_back.png"));

    public static readonly Dictionary<string, SpecialCustomerModel> SPECIAL_CUSTOMER_MODELS = new Dictionary<string, SpecialCustomerModel>
    {
        { DEAN_IDENTIFIER, DEAN },
        { TEAMPLAY_MVP_IDENTIFIER, TEAMPLAY_MVP },
        { BROKEN_GIRL_IDENTIFIER, BROKEN_GIRL },
        { COUPLE_IDENTIFIER, COUPLE },
        { SOLO_EATER_IDENTIFIER, SOLO_EATER },
        { FRESHMAN_IDENTIFIER, FRESHMAN },
        { RAIN_WETTED_IDENTIFIER, RAIN_WETTED },
        { F_GIVER_PROF_IDENTIFIER, F_GIVER_PROF },
        { TEST_PREPARER_IDENTIFIER, TEST_PREPARER },
        { EARLY_BIRD_IDENTIFIER, EARLY_BIRD },
        { APPLY_FAILER_IDENTIFIER, APPLY_FAILER },
        { SENIOR_AND_JUNIOR_IDENTIFIER, SENIOR_AND_JUNIOR },
        { GRASS_ENJOYER_IDENTIFIER, GRASS_ENJOYER },
        { TOP_STUDENT_IDENTIFIER, TOP_STUDENT },
        { DEVELOPER_IDENTIFIER, DEVELOPER }
    };

    public static SpecialCustomerModel Get(string identifier)
    {
        return SPECIAL_CUSTOMER_MODELS[identifier];
    }
    public static string[] RegisteredIdentifiers
    {
        get
        {
            return SpecialCustomerRegistry.SPECIAL_CUSTOMER_MODELS.Keys.ToArray();
        }
    }
}

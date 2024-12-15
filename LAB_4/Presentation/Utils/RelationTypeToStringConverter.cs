using System;
using System.Globalization;
using System.Windows.Data;
using LAB_4.DAL.Models.Enums;

namespace LAB_4.Presentation.Utils
{
    public class RelationTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RelationType relationType)
            {
                return relationType switch
                {
                    RelationType.Parent => "Родитель",
                    RelationType.Child => "Ребенок",
                    RelationType.Sibling => "Брат/Сестра",
                    RelationType.Spouse => "Супруг/Супруга",
                    RelationType.Grandparent => "Дедушка/Бабушка",
                    RelationType.Grandchild => "Внук/Внучка",
                    RelationType.AuntUncle => "Тетя/Дядя",
                    RelationType.NephewNiece => "Племянник/Племянница",
                    RelationType.Cousin => "Кузен/Кузина",
                    RelationType.ParentInLaw => "Тесть/Теща, Свекр/Свекровь",
                    RelationType.ChildInLaw => "Пасынок/Падчерица",
                    RelationType.SiblingInLaw => "Шурин/Свояк, Золовка/Свояченица",
                    _ => "Неизвестно",
                };
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.TryParse(typeof(RelationType), value as string, out var result) ? result : RelationType.Parent;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Xamarin.Forms.DynamicLayout
{
    public class DynamicLayout : StackLayout
    {
        public IEnumerable<object> Source
        {
            get { return (IEnumerable<object>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly BindableProperty SourceProperty =
             BindableProperty.Create("Source", typeof(IEnumerable<object>), typeof(DynamicLayout), null, propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(DynamicLayout), default(DataTemplate), propertyChanged: OnItemTemplateChanged);

        private static object CreateContent(DataTemplate template, object item, BindableObject container)
        {
            if (template is DataTemplateSelector selector)
            {
                template = selector.SelectTemplate(item, container);
            }

            return template.CreateContent();
        }

        private View CreateView(object item)
        {
            var view = (View)CreateContent(ItemTemplate, item, this);
            view.BindingContext = item;
            return view;
        }

        private void AddItems()
        {
            var items = Source;
            if (items == null || ItemTemplate == null)
            {
                return;
            }

            var children = Children;
            children.Clear();

            foreach (var item in items)
            {
                children.Add(CreateView(item));
            }
        }

        private static void OnItemsSourceChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var layout = (DynamicLayout)bindableObject;
            if (newValue == null)
            {
                return;
            }

            layout.AddItems();
        }

        private static void OnItemTemplateChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var layout = (DynamicLayout)bindableObject;
            if (newValue == null)
            {
                return;
            }

            layout.AddItems();
        }

       
    }
}

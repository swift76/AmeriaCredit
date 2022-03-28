import { registerLocale, setDefaultLocale } from 'react-datepicker';
import hy from 'date-fns/locale/hy'; // the locale you want

registerLocale('hy', hy);

setDefaultLocale('hy');

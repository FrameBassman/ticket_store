import React from 'react';
import Fab from '@material-ui/core/Fab';
import CheckIcon from '@material-ui/icons/Check';
import CancelIcon from '@material-ui/icons/Cancel';
import ScannerIcon from '@material-ui/icons/Scanner';
import { createMuiTheme, MuiThemeProvider } from '@material-ui/core/styles';
import green from '@material-ui/core/colors/green';
import red from '@material-ui/core/colors/red';
import yellow from '@material-ui/core/colors/yellow';
import './Status.css';

const waitableTheme = createMuiTheme({
  palette: {
    primary: green,
    secondary: red,
  }
});
const nonWaitableTheme = createMuiTheme({
  palette: {
    primary: yellow,
  }
});

const Description = (props: any) => {
  return (
    <span className="description">{props.message}</span>
  )
}

export const Status = (props: any) => {
  const { className, pass, wait } = props;
  if (wait) {
    return (
      <MuiThemeProvider theme={waitableTheme}>
        <div className={className}>
          {pass
            ? (
              <div>
                <Fab color="primary"><CheckIcon /></Fab>
                <Description message="Успешно!"/>
              </div>
            )
            : (
              <div>
                <Fab color="secondary"><CancelIcon /></Fab>
                <Description message="Ошибочка вышла!"/>
              </div>
            )
          }
        </div>
      </MuiThemeProvider>
    )
  } else {
    return (
      <MuiThemeProvider theme={nonWaitableTheme}>
        <div className={className}>
          <Fab color="primary"><ScannerIcon /></Fab>
          <Description message="Готов сканировать!"/>
        </div>
      </MuiThemeProvider>
    )
  }
}
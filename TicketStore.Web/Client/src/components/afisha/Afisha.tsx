import React, { Component } from 'react';
import { connect } from 'react-redux';
import { eventsFetchData } from '../../store/Afisha/events/actions';
import { AfishaState } from '../../store/Afisha/state';

import Event from './Event';

import { withStyles } from '@material-ui/styles';
import { styles } from './Afisha.styles';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import Box from '@material-ui/core/Box';
import { CenteredProgress } from '../core/progress/CenteredProgress';


class Afisha extends Component<any, AfishaState> {
  componentDidMount() {
    this.props.fetchEvents();
  }

  render() {
    const { classes, events, hasErrored, isLoading } = this.props;
    if (hasErrored) {
      return (
        <Typography align="center" component="div">
          <Box marginTop={16}>
            <div>У нас что-то сломалось.</div>
            <div>Мы уже знаем об этом и уже чиним.</div>
          </Box>
        </Typography>
      );
    }

    if (isLoading) {
      return <CenteredProgress />
    }

    return (
      <div className={classes.afisha}>
        <Grid container justify="center">
          {events.map((event, key) => (
            <Event
              key={key}
              artist={event.artist}
              roubles={event.roubles}
              pressRelease={event.pressRelease}
              yandexMoneyAccount={event.yandexMoneyAccount}
              time={event.time}
              posterUrl={event.posterUrl}
            />
          ))}
        </Grid>
      </div>
    )
  }
}

const mapStateToProps = (state) => {
  return {
    events: state.afisha.events,
    hasErrored: state.afisha.eventsHasErrored,
    isLoading: state.afisha.eventsIsLoading
  };
};
const mapDispatchToProps = (dispatch) => {
  return {
    fetchEvents: () => dispatch(eventsFetchData())
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(Afisha));

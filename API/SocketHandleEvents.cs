﻿using DNet.API.Gateway;
using DNet.Structures.Channels;
using DNet.Structures.Guilds;
using DNet.Structures.Messages;
using System;

namespace DNet.API
{
    public sealed partial class SocketHandle
    {
        // TODO
        // Discord Events
        public event EventHandler OnRateLimit;
        public event EventHandler<ReadyEvent> OnReady;
        public event EventHandler OnResumed;
        public event EventHandler<Guild> OnGuildCreate;
        public event EventHandler<Guild> OnGuildUpdate;
        public event EventHandler<UnavailableGuild> OnGuildDelete;
        //public event EventHandler<UnavailableGuild> OnGuildUnavailable;
        //public event EventHandler<Guild> OnGuildAvailable;
        public event EventHandler OnGuildMemberAdd;
        public event EventHandler OnGuildMemberRemove;
        public event EventHandler OnGuildMemberUpdate;
        public event EventHandler OnGuildMemberAvailable;
        public event EventHandler OnGuildMemberSpeaking;
        public event EventHandler OnGuildMembersChunk;
        public event EventHandler OnGuildIntegrationsUpdate;
        // ...
        public event EventHandler<GenericMessage> OnMessageCreate;
        public event EventHandler<MessageDeleteEvent> OnMessageDelete;
        public event EventHandler<MessageDeleteBulkEvent> OnMessageDeleteBulk;
        public event EventHandler<GenericMessage> OnMessageUpdate;
        public event EventHandler<MessageReactionEvent> OnMessageReactionAdd;
        public event EventHandler<MessageReactionEvent> OnMessageReactionRemove;
        public event EventHandler<MessageReactionRemoveAllEvent> OnMessageReactionRemoveAll;
        // ...
        public event EventHandler<PresenceUpdateEvent> OnPresenceUpdate;

        public event EventHandler<TypingStartEvent> OnTypingStart;

        public event EventHandler<UserUpdateEvent> OnUserUpdate;

        public event EventHandler<Channel> OnChannelCreate;
    }
}
